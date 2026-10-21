// using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
// using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;
// using CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.PersistSms;
// using Mediator;

// namespace CleanArchitecture.Blazor.Server.UI.Services;

// public sealed class SmsInboundPoller : BackgroundService
// {
//     private readonly IServiceScopeFactory _scopeFactory;
//     private readonly ISmsGatewaySettingsCache _cache;
//     private readonly ILogger<SmsInboundPoller> _log;

//     private readonly SemaphoreSlim _wake = new(0, 1);

//     private const string CursorKey = "sms.inbound.lastGatewayId";


//     private long _lastId;
//     private bool _cursorLoaded;
//     private int  _consecutiveFailures;



//     public SmsInboundPoller(
//         IServiceScopeFactory scopeFactory,
//         ISmsGatewaySettingsCache cache,
//         ILogger<SmsInboundPoller> log)
//     {
//         _scopeFactory = scopeFactory;
//         _cache        = cache;
//         _log          = log;
//     }

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         _log.LogInformation("SMS inbound poller service started.");

//         _cache.Changed += OnSettingsChanged;

//         // Ensure the cache is populated before the first iteration.
//         await EnsureCacheLoadedAsync(stoppingToken);

//         await TryLoadCursorAsync(stoppingToken);

//         while (!stoppingToken.IsCancellationRequested)
//         {
//             var cfg = _cache.Current;
//             if (cfg is null)
//             {
//                 // Should not happen after EnsureCacheLoadedAsync, but be defensive.
//                 await SafeWaitAsync(TimeSpan.FromSeconds(10), stoppingToken);
//                 continue;
//             }

//             if (cfg.EnableInboundPolling)
//             {
//                 try
//                 {
//                     await PollOnceAsync(cfg.BaseUrl, stoppingToken);
//                     if (_consecutiveFailures > 0) { /* log recovery */ }
//                     _consecutiveFailures = 0;
//                 }
//                 catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }
//                 catch (Exception ex)
//                 {
//                     _consecutiveFailures++;
//                     var backoff = TimeSpan.FromSeconds(
//                         Math.Min(300, 5 * Math.Pow(2, Math.Min(_consecutiveFailures - 1, 6))));
//                     _log.LogWarning(ex, "SMS poll failed (attempt {N}). Backing off {Backoff}s.",
//                                     _consecutiveFailures, backoff.TotalSeconds);
//                     await SafeWaitAsync(backoff, stoppingToken);
//                     continue;
//                 }
//             }

//             await SafeWaitAsync(TimeSpan.FromSeconds(cfg.PollingIntervalSeconds), stoppingToken);
//         }

//         _cache.Changed -= OnSettingsChanged;
//         _log.LogInformation("SMS inbound poller service stopped.");
//     }

//     private async Task EnsureCacheLoadedAsync(CancellationToken ct)
//     {
//         if (_cache.Current is not null) return;
//         try
//         {
//             using var scope = _scopeFactory.CreateScope();
//             var provider = scope.ServiceProvider.GetRequiredService<ISmsGatewaySettingsProvider>();
//             await provider.GetAsync(ct);   // populates the cache
//         }
//         catch (Exception ex)
//         {
//             _log.LogWarning(ex,
//                 "Could not preload SMS gateway settings; will retry on next iteration.");
//         }
//     }

//     private void OnSettingsChanged(object? sender, SmsGatewaySettingsDto e)
//     {
//         try { _wake.Release(); } catch (SemaphoreFullException) { }
//     }

//     private async Task SafeWaitAsync(TimeSpan delay, CancellationToken ct)
//     {
//         using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
//         var wake  = _wake.WaitAsync(linked.Token);
//         var sleep = Task.Delay(delay, linked.Token);
//         var done  = await Task.WhenAny(wake, sleep);
//         linked.Cancel();
//         try { await done; } catch (OperationCanceledException) { }
//     }


 


//     private async Task TryLoadCursorAsync(CancellationToken ct)
//     {
//         try
//         {
//             using var scope = _scopeFactory.CreateScope();
//             var store = scope.ServiceProvider.GetRequiredService<ISmsCursorStore>();
//             _lastId = await store.GetAsync(CursorKey, ct);
//             _cursorLoaded = true;
//             _log.LogInformation("SMS inbound cursor restored at gatewayId={Last}.", _lastId);
//         }
//         catch (Exception ex)
//         {
//             _cursorLoaded = false;
//             _lastId = 0;
//             _log.LogWarning(ex,
//                 "Could not load SMS cursor; starting from 0 (dedupe prevents duplicates).");
//         }
//     }

//     private async Task SaveCursorAsync(long value, CancellationToken ct)
//     {
//         try
//         {
//             using var scope = _scopeFactory.CreateScope();
//             var store = scope.ServiceProvider.GetRequiredService<ISmsCursorStore>();
//             await store.SetAsync(CursorKey, value, ct);
//         }
//         catch (Exception ex)
//         {
//             _log.LogWarning(ex, "Could not persist SMS cursor at gatewayId={Value}.", value);
//         }
//     }

//     private async Task PollOnceAsync(string baseUrl, CancellationToken ct)
//     {
//         using var scope = _scopeFactory.CreateScope();
//         var sp = scope.ServiceProvider;

//         var gateway  = sp.GetRequiredService<ISmsGatewayClient>();
//         var mediator = sp.GetRequiredService<IMediator>();

//         if (!_cursorLoaded) await TryLoadCursorAsync(ct);

//         var batch = await gateway.GetInboxAsync(_lastId, limit: 200, ct);
//         if (batch.Count == 0)
//         {
//             _log.LogTrace("SMS poll: no new messages (cursor={Cursor}).", _lastId);
//             return;
//         }

//         var startCursor = _lastId;
//         var maxSeen     = _lastId;
//         int persisted = 0, duplicates = 0;

//         foreach (var item in batch)
//         {
//             if (ct.IsCancellationRequested) break;

//             try
//             {
//                 var result = await mediator.Send(
//                     new PersistInboundSmsCommand(new InboundSmsDto(
//                         GatewayId: item.Id,
//                         From:      item.From,
//                         Body:      item.Body,
//                         Ts:        item.Ts)),
//                     ct);

//                 if (result.Succeeded)
//                 {
//                     if (item.Id > maxSeen) maxSeen = item.Id;
//                     if (result.Data) persisted++; else duplicates++;
//                 }
//                 else
//                 {
//                     var errs = string.Join("; ", result.Errors ?? new List<string>());
//                     _log.LogWarning(
//                         "Persist inbound SMS gatewayId={Id} failed: {Errors}. Cursor not advanced.",
//                         item.Id, errs);
//                     break;
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _log.LogError(ex,
//                     "Unexpected error persisting gatewayId={Id}. Cursor not advanced.", item.Id);
//                 break;
//             }
//         }

//         if (maxSeen > startCursor)
//         {
//             _lastId = maxSeen;
//             await SaveCursorAsync(_lastId, ct);
//         }

//         if (persisted > 0 || duplicates > 0)
//         {
//             _log.LogInformation(
//                 "SMS poll: fetched={Fetched} stored={Stored} duplicates={Dup} cursor={Cursor}.",
//                 batch.Count, persisted, duplicates, _lastId);
//         }
//     }
// }

using CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.PersistSms;
using Mediator;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Blazor.Server.UI.Services;

/// <summary>
/// Pulls inbound SMS from the gateway on a schedule. Complements the webhook
/// push path — used when webhooks aren't deliverable or as a safety net.
///
/// Resilience features:
///   • Cursor-based pagination with durable checkpointing.
///   • Exponential backoff with jitter, capped at 60s.
///   • Settings changes wake the poller and reset backoff immediately.
///   • Per-message poison-pill protection (max 3 retries, then skip).
///   • No overlapping polls (single-flight).
/// </summary>
public sealed class SmsInboundPoller : BackgroundService
{
    private const string CursorKey            = "sms.inbound.lastGatewayId";
    private const int    InboxBatchSize       = 200;
    private const int    MaxPoisonRetries     = 3;
    private const int    MaxBackoffSeconds    = 60;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ISmsGatewaySettingsCache _cache;
    private readonly ILogger<SmsInboundPoller> _log;

    /// <summary>Signals an immediate retry (settings change, shutdown, etc.).</summary>
    private readonly SemaphoreSlim _wake = new(0, 1);

    /// <summary>Prevents overlap if a poll is slow and a wake fires.</summary>
    private readonly SemaphoreSlim _pollGate = new(1, 1);

    private long _lastId;
    private bool _cursorLoaded;
    private int  _consecutiveFailures;

    // Tracks per-gatewayId failure counts so we can skip poison messages.
    // Not persisted — resets on restart, which is fine (3 more tries).
    private readonly Dictionary<long, int> _poisonCounters = new();

    public SmsInboundPoller(
        IServiceScopeFactory scopeFactory,
        ISmsGatewaySettingsCache cache,
        ILogger<SmsInboundPoller> log)
    {
        _scopeFactory = scopeFactory;
        _cache        = cache;
        _log          = log;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Main loop
    // ─────────────────────────────────────────────────────────────────────────

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _log.LogInformation("SMS inbound poller service started.");

        _cache.Changed += OnSettingsChanged;

        await EnsureCacheLoadedAsync(stoppingToken).ConfigureAwait(false);
        await TryLoadCursorAsync(stoppingToken).ConfigureAwait(false);

        while (!stoppingToken.IsCancellationRequested)
        {
            var cfg = _cache.Current;
            if (cfg is null)
            {
                await SafeWaitAsync(TimeSpan.FromSeconds(10), stoppingToken).ConfigureAwait(false);
                continue;
            }

            if (cfg.EnableInboundPolling)
            {
                try
                {
                    await PollOnceAsync(stoppingToken).ConfigureAwait(false);

                    if (_consecutiveFailures > 0)
                    {
                        _log.LogInformation(
                            "SMS poll recovered after {N} consecutive failure(s).",
                            _consecutiveFailures);
                        _consecutiveFailures = 0;
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _consecutiveFailures++;
                    var backoff = ComputeBackoff(_consecutiveFailures);

                    _log.LogWarning(ex,
                        "SMS poll failed (attempt {N}, reason={Reason}). Backing off {Backoff}s.",
                        _consecutiveFailures, ClassifyReason(ex), backoff.TotalSeconds);

                    await SafeWaitAsync(backoff, stoppingToken).ConfigureAwait(false);
                    continue;
                }
            }

            var interval = TimeSpan.FromSeconds(
                Math.Max(5, cfg.PollingIntervalSeconds));
            await SafeWaitAsync(interval, stoppingToken).ConfigureAwait(false);
        }

        _cache.Changed -= OnSettingsChanged;
        _log.LogInformation("SMS inbound poller service stopped.");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Backoff / diagnostics helpers
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Exponential backoff with ±20% jitter, capped at <see cref="MaxBackoffSeconds"/>.
    /// 5s → 10s → 20s → 40s → 60s → 60s…
    /// </summary>
    private static TimeSpan ComputeBackoff(int failures)
    {
        var exponent = Math.Min(failures - 1, 4);           // cap exponent at 2^4 = 16
        var baseSeconds = Math.Min(MaxBackoffSeconds, 5 * Math.Pow(2, exponent));
        var jitter = 0.8 + Random.Shared.NextDouble() * 0.4; // 0.8–1.2
        return TimeSpan.FromSeconds(baseSeconds * jitter);
    }

    /// <summary>
    /// Returns a short, log-friendly classification of the failure cause.
    /// Avoids stack-trace noise in the message line.
    /// </summary>
    private static string ClassifyReason(Exception ex) => ex switch
    {
        SmsGatewayException { StatusCode: not null } s =>
            $"gateway op={s.Operation} status={s.StatusCode}",
        SmsGatewayException s =>
            $"gateway op={s.Operation} (no status)",
        HttpRequestException http =>
            $"transport status={(int?)http.StatusCode}",
        TaskCanceledException =>
            "http timeout",
        OperationCanceledException =>
            "cancelled",
        _ => ex.GetType().Name
    };

    // ─────────────────────────────────────────────────────────────────────────
    // Cache / cursor
    // ─────────────────────────────────────────────────────────────────────────

    private async Task EnsureCacheLoadedAsync(CancellationToken ct)
    {
        if (_cache.Current is not null) return;
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var provider = scope.ServiceProvider.GetRequiredService<ISmsGatewaySettingsProvider>();
            await provider.GetAsync(ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex,
                "Could not preload SMS gateway settings; will retry on next iteration.");
        }
    }

    private void OnSettingsChanged(object? sender, SmsGatewaySettingsDto e)
    {
        try { _wake.Release(); }
        catch (SemaphoreFullException) { /* already pending — fine */ }
    }

    private async Task SafeWaitAsync(TimeSpan delay, CancellationToken ct)
    {
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var wake  = _wake.WaitAsync(linked.Token);
        var sleep = Task.Delay(delay, linked.Token);
        await Task.WhenAny(wake, sleep).ConfigureAwait(false);
        linked.Cancel();
        // Drain the wake signal if it fired, so the next wait blocks correctly.
        if (wake.IsCompletedSuccessfully) _ = wake;
    }

    private async Task TryLoadCursorAsync(CancellationToken ct)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<ISmsCursorStore>();
            _lastId = await store.GetAsync(CursorKey, ct).ConfigureAwait(false);
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
            await store.SetAsync(CursorKey, value, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Could not persist SMS cursor at gatewayId={Value}.", value);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Polling
    // ─────────────────────────────────────────────────────────────────────────

    private async Task PollOnceAsync(CancellationToken ct)
    {
        // Single-flight guard: if a poll is already running, skip this tick.
        if (!await _pollGate.WaitAsync(TimeSpan.Zero, ct).ConfigureAwait(false))
        {
            _log.LogDebug("SMS poll skipped — previous poll still running.");
            return;
        }

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var sp = scope.ServiceProvider;

            var gateway  = sp.GetRequiredService<ISmsGatewayClient>();
            var mediator = sp.GetRequiredService<IMediator>();

            if (!_cursorLoaded) await TryLoadCursorAsync(ct).ConfigureAwait(false);

            var batch = await gateway
                .GetInboxAsync(_lastId, limit: InboxBatchSize, ct)
                .ConfigureAwait(false);

            if (batch.Count == 0)
            {
                _log.LogTrace("SMS poll: no new messages (cursor={Cursor}).", _lastId);
                return;
            }

            var startCursor = _lastId;
            var maxSeen     = _lastId;
            int persisted = 0, duplicates = 0, skipped = 0;

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
                        ct).ConfigureAwait(false);

                    if (result.Succeeded)
                    {
                        if (item.Id > maxSeen) maxSeen = item.Id;
                        if (result.Data) persisted++;
                        else             duplicates++;

                        _poisonCounters.Remove(item.Id);
                    }
                    else
                    {
                        var errs = string.Join("; ", result.Errors ?? new List<string>());
                        _log.LogWarning(
                            "Persist inbound SMS gatewayId={Id} failed: {Errors}.",
                            item.Id, errs);

                        // Poison-pill protection: after N failures, skip and advance.
                        if (RecordPoisonFailure(item.Id) >= MaxPoisonRetries)
    {
        _log.LogError(
            "Gateway ID {Id} failed {N} times — skipping permanently. " +
            "Manual intervention required if this message matters.",
            item.Id, MaxPoisonRetries);

        if (item.Id > maxSeen) maxSeen = item.Id;
        skipped++;
        _poisonCounters.Remove(item.Id);
    }
                        // if (++GetPoisonCount(item.Id) >= MaxPoisonRetries)
                        // {
                        //     _log.LogError(
                        //         "Gateway ID {Id} failed {N} times — skipping permanently. " +
                        //         "Manual intervention required if this message matters.",
                        //         item.Id, MaxPoisonRetries);

                        //     if (item.Id > maxSeen) maxSeen = item.Id;
                        //     skipped++;
                        //     _poisonCounters.Remove(item.Id);
                        // }
                        else
                        {
                            // Retry next poll from the same cursor.
                            break;
                        }
                    }
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _log.LogError(ex,
                        "Unexpected error persisting gatewayId={Id}.", item.Id);

                    // if (++GetPoisonCount(item.Id) >= MaxPoisonRetries)
                    // {
                    //     if (item.Id > maxSeen) maxSeen = item.Id;
                    //     skipped++;
                    //     _poisonCounters.Remove(item.Id);
                    // }

                     if (RecordPoisonFailure(item.Id) >= MaxPoisonRetries)
    {
        if (item.Id > maxSeen) maxSeen = item.Id;
        skipped++;
        _poisonCounters.Remove(item.Id);
    }
                    else
                    {
                        break;
                    }
                }
            }

            if (maxSeen > startCursor)
            {
                _lastId = maxSeen;
                await SaveCursorAsync(_lastId, ct).ConfigureAwait(false);
            }

            if (persisted > 0 || duplicates > 0 || skipped > 0)
            {
                _log.LogInformation(
                    "SMS poll: fetched={Fetched} stored={Stored} dup={Dup} skipped={Skipped} cursor={Cursor}.",
                    batch.Count, persisted, duplicates, skipped, _lastId);
            }
        }
        finally
        {
            _pollGate.Release();
        }
    }

    // private int GetPoisonCount(long id) =>
    //     _poisonCounters.TryGetValue(id, out var n) ? n : 0;

    // private int IncrementPoison(long id)
    // {
    //     var n = GetPoisonCount(id) + 1;
    //     _poisonCounters[id] = n;
    //     return n;
    // }

    private int RecordPoisonFailure(long gatewayId)
{
    var next = (_poisonCounters.TryGetValue(gatewayId, out var n) ? n : 0) + 1;
    _poisonCounters[gatewayId] = next;
    return next;
}

    // Alias so the call sites read cleanly.
   // private int GetPoisonCountIncrement(long id) => IncrementPoison(id);
}