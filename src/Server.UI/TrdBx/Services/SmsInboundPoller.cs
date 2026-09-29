using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.PersistSms;
using Mediator;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Blazor.Server.UI.Services;

public sealed class SmsInboundPoller : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ISmsGatewaySettingsCache _cache;
    private readonly ILogger<SmsInboundPoller> _log;

    private readonly SemaphoreSlim _wake = new(0, 1);

    private const string CursorKey = "sms.inbound.lastGatewayId";


    private long _lastId;
    private bool _cursorLoaded;
    private int  _consecutiveFailures;



    public SmsInboundPoller(
        IServiceScopeFactory scopeFactory,
        ISmsGatewaySettingsCache cache,
        ILogger<SmsInboundPoller> log)
    {
        _scopeFactory = scopeFactory;
        _cache        = cache;
        _log          = log;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _log.LogInformation("SMS inbound poller service started.");

        _cache.Changed += OnSettingsChanged;

        // Ensure the cache is populated before the first iteration.
        await EnsureCacheLoadedAsync(stoppingToken);

        await TryLoadCursorAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var cfg = _cache.Current;
            if (cfg is null)
            {
                // Should not happen after EnsureCacheLoadedAsync, but be defensive.
                await SafeWaitAsync(TimeSpan.FromSeconds(10), stoppingToken);
                continue;
            }

            if (cfg.EnableInboundPolling)
            {
                try
                {
                    await PollOnceAsync(cfg.BaseUrl, stoppingToken);
                    if (_consecutiveFailures > 0) { /* log recovery */ }
                    _consecutiveFailures = 0;
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }
                catch (Exception ex)
                {
                    _consecutiveFailures++;
                    var backoff = TimeSpan.FromSeconds(
                        Math.Min(300, 5 * Math.Pow(2, Math.Min(_consecutiveFailures - 1, 6))));
                    _log.LogWarning(ex, "SMS poll failed (attempt {N}). Backing off {Backoff}s.",
                                    _consecutiveFailures, backoff.TotalSeconds);
                    await SafeWaitAsync(backoff, stoppingToken);
                    continue;
                }
            }

            await SafeWaitAsync(TimeSpan.FromSeconds(cfg.PollingIntervalSeconds), stoppingToken);
        }

        _cache.Changed -= OnSettingsChanged;
        _log.LogInformation("SMS inbound poller service stopped.");
    }

    private async Task EnsureCacheLoadedAsync(CancellationToken ct)
    {
        if (_cache.Current is not null) return;
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var provider = scope.ServiceProvider.GetRequiredService<ISmsGatewaySettingsProvider>();
            await provider.GetAsync(ct);   // populates the cache
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex,
                "Could not preload SMS gateway settings; will retry on next iteration.");
        }
    }

    private void OnSettingsChanged(object? sender, SmsGatewaySettingsDto e)
    {
        try { _wake.Release(); } catch (SemaphoreFullException) { }
    }

    private async Task SafeWaitAsync(TimeSpan delay, CancellationToken ct)
    {
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var wake  = _wake.WaitAsync(linked.Token);
        var sleep = Task.Delay(delay, linked.Token);
        var done  = await Task.WhenAny(wake, sleep);
        linked.Cancel();
        try { await done; } catch (OperationCanceledException) { }
    }


 


    private async Task TryLoadCursorAsync(CancellationToken ct)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<ISmsCursorStore>();
            _lastId = await store.GetAsync(CursorKey, ct);
            _cursorLoaded = true;
            _log.LogInformation("SMS inbound cursor restored at gatewayId={Last}.", _lastId);
        }
        catch (Exception ex)
        {
            _cursorLoaded = false;
            _lastId = 0;
            _log.LogWarning(ex,
                "Could not load SMS cursor; starting from 0 (dedupe prevents duplicates).");
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
            _log.LogWarning(ex, "Could not persist SMS cursor at gatewayId={Value}.", value);
        }
    }

    private async Task PollOnceAsync(string baseUrl, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var sp = scope.ServiceProvider;

        var gateway  = sp.GetRequiredService<ISmsGatewayClient>();
        var mediator = sp.GetRequiredService<IMediator>();

        if (!_cursorLoaded) await TryLoadCursorAsync(ct);

        var batch = await gateway.GetInboxAsync(_lastId, limit: 200, ct);
        if (batch.Count == 0)
        {
            _log.LogTrace("SMS poll: no new messages (cursor={Cursor}).", _lastId);
            return;
        }

        var startCursor = _lastId;
        var maxSeen     = _lastId;
        int persisted = 0, duplicates = 0;

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
                    if (item.Id > maxSeen) maxSeen = item.Id;
                    if (result.Data) persisted++; else duplicates++;
                }
                else
                {
                    var errs = string.Join("; ", result.Errors ?? new List<string>());
                    _log.LogWarning(
                        "Persist inbound SMS gatewayId={Id} failed: {Errors}. Cursor not advanced.",
                        item.Id, errs);
                    break;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex,
                    "Unexpected error persisting gatewayId={Id}. Cursor not advanced.", item.Id);
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
                "SMS poll: fetched={Fetched} stored={Stored} duplicates={Dup} cursor={Cursor}.",
                batch.Count, persisted, duplicates, _lastId);
        }
    }
}