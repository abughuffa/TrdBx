
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.PersistSms;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Infrastructure.Configurations;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Blazor.Server.UI.Services;

public sealed class SmsInboundPoller : BackgroundService
{
    private const string CursorKey = "sms.inbound.lastGatewayId";

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SmsGatewayOptions _opt;
    private readonly ILogger<SmsInboundPoller> _log;

    // Cursor is read once at startup and kept in memory; written back to
    // the store only when it advances.
    private long _lastId;
    private bool _cursorLoaded;

    // Failure tracking for exponential backoff.
    private int _consecutiveFailures;

    public SmsInboundPoller(
        IServiceScopeFactory scopeFactory,
        IOptions<SmsGatewayOptions> opt,
        ILogger<SmsInboundPoller> log)
    {
        _scopeFactory = scopeFactory;
        _opt          = opt.Value;
        _log          = log;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_opt.EnableInboundPolling)
        {
            _log.LogInformation("SMS inbound polling is disabled (SmsGateway:EnableInboundPolling=false).");
            return;
        }

        _log.LogInformation(
            "SMS inbound poller started. Base={Base} Interval={Interval}s",
            _opt.BaseUrl, _opt.PollingIntervalSeconds);

        // Resolve the cursor once. If the phone is offline we just log and
        // continue with 0 — the first successful poll will move it forward.
        await TryLoadCursorAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PollOnceAsync(stoppingToken);

                // Success: reset failure counter.
                if (_consecutiveFailures > 0)
                {
                    _log.LogInformation(
                        "SMS poll recovered after {Count} failure(s).",
                        _consecutiveFailures);
                    _consecutiveFailures = 0;
                }

                // Normal cadence.
                await Task.Delay(
                    TimeSpan.FromSeconds(_opt.PollingIntervalSeconds),
                    stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _consecutiveFailures++;

                // Backoff: 5s, 10s, 20s, 40s, 80s, 160s, capped at 5 minutes.
                var backoff = TimeSpan.FromSeconds(
                    Math.Min(300, 5 * Math.Pow(2, Math.Min(_consecutiveFailures - 1, 6))));

                _log.LogWarning(ex,
                    "SMS poll failed (attempt {N}). Backing off {Backoff}s.",
                    _consecutiveFailures, backoff.TotalSeconds);

                try { await Task.Delay(backoff, stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }

        _log.LogInformation("SMS inbound poller stopped.");
    }

    // ──────────────────────────────────────────────────────────────────
    // Cursor
    // ──────────────────────────────────────────────────────────────────
    private async Task TryLoadCursorAsync(CancellationToken ct)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<ISmsCursorStore>();

            _lastId       = await store.GetAsync(CursorKey, ct);
            _cursorLoaded = true;

            _log.LogInformation(
                "SMS inbound cursor restored at gatewayId={Last}.", _lastId);
        }
        catch (Exception ex)
        {
            // Non-fatal: fall back to 0, which means the first poll will
            // re-scan the whole inbox. The handler dedupes by gateway id,
            // so this only costs bandwidth, not correctness.
            _cursorLoaded = false;
            _lastId       = 0;
            _log.LogWarning(ex,
                "Could not load SMS cursor; starting from 0 (dedupe will prevent duplicates).");
        }
    }

    private async Task SaveCursorAsync(long value, CancellationToken ct)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<ISmsCursorStore>();
            await store.SetAsync(CursorKey, value, ct);
        }
        catch (Exception ex)
        {
            // Non-fatal: the next restart will re-scan. Log and move on.
            _log.LogWarning(ex,
                "Could not persist SMS cursor at gatewayId={Value}.", value);
        }
    }

    // ──────────────────────────────────────────────────────────────────
    // One poll iteration
    // ──────────────────────────────────────────────────────────────────
    private async Task PollOnceAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var sp = scope.ServiceProvider;

        var gateway = sp.GetRequiredService<ISmsGatewayClient>();
        var mediator = sp.GetRequiredService<IMediator>();

        // Make sure the cursor is loaded before the first call.
        if (!_cursorLoaded)
            await TryLoadCursorAsync(ct);

        // Pull up to 200 rows newer than the last seen id.
        var batch = await gateway.GetInboxAsync(_lastId, limit: 200, ct);

        if (batch.Count == 0)
        {
            _log.LogTrace("SMS poll: no new messages (cursor={Cursor}).", _lastId);
            return;
        }

        var startCursor = _lastId;
        var maxSeen     = _lastId;
        var persisted   = 0;
        var duplicates  = 0;

        foreach (var item in batch)
        {
            if (ct.IsCancellationRequested) break;

            try
            {
                var result = await mediator.Send(
                    new PersistInboundSmsCommand(new InboundSmsDto(
                        GatewayId: item.Id,
                        From:      item.From,
                        Body:      item.Body,
                        Ts:        item.Ts)),
                    ct);

                if (result.Succeeded)
                {
                    // Move cursor regardless of whether the row was new
                    // (true) or already present (false). We've seen this id.
                    if (item.Id > maxSeen) maxSeen = item.Id;

                    if (result.Data) persisted++;
                    else             duplicates++;
                }
                else
                {
                    // Handler reported a failure for this item. Do NOT advance
                    // the cursor past it, so the next poll retries.
                    var errs = string.Join("; ", result.Errors ?? new List<string>());
                    _log.LogWarning(
                        "Persist inbound SMS gatewayId={GatewayId} failed: {Errors}. " +
                        "Cursor will not advance past this id.",
                        item.Id, errs);
                    break;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex,
                    "Unexpected error persisting inbound SMS gatewayId={GatewayId}. " +
                    "Cursor will not advance past this id.",
                    item.Id);
                break;
            }
        }

        if (maxSeen > startCursor)
        {
            _lastId = maxSeen;
            await SaveCursorAsync(_lastId, ct);
        }

        if (persisted > 0 || duplicates > 0)
        {
            _log.LogInformation(
                "SMS poll: fetched={Fetched} stored={Stored} duplicates={Duplicates} cursor={Cursor}.",
                batch.Count, persisted, duplicates, _lastId);
        }
    }
}