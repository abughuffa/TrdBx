using CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;
using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.PersistSms;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using Mediator;


namespace CleanArchitecture.Blazor.Server.UI.Services;

/// <summary>
/// Single background worker that combines the old <c>SmsInboundPoller</c> and
/// <c>SmsInboundPartSweeper</c> into one loop.
///
/// <see cref="SmsGatewaySettingsDto.EnableInboundPolling"/> is the master
/// switch for the entire pipeline. When false, the loop idles — neither the
/// poller nor the sweeper runs. The five sweeper tunables are read fresh on
/// every tick, so changes take effect on the next wake.
///
/// Phases (when polling is enabled):
///   • Poll  — every <c>PollingIntervalSeconds</c>, pull new inbound SMS
///             from the gateway and persist them. Exponential backoff with
///             jitter; poison-pill protection; cursor checkpointing;
///             single-flight guard.
///   • Sweep — every <c>SweeperIntervalSeconds</c>, (a) re-trigger assembly
///             of any stranded multipart group, (b) delete orphaned parts,
///             (c) prune assembled parts past retention.
/// </summary>
public sealed class SmsInboundService : BackgroundService
{
    // ── Poller constants ─────────────────────────────────────────────────
    private const string CursorKey         = "sms.inbound.lastGatewayId";
    private const int    InboxBatchSize    = 200;
    private const int    MaxPoisonRetries  = 3;
    private const int    MaxBackoffSeconds = 60;

    /// <summary>Cadence at which the loop re-checks the cache while disabled.</summary>
    private static readonly TimeSpan PollingDisabledRecheck = TimeSpan.FromSeconds(30);

    /// <summary>Delay before the very first sweep, so startup isn't noisy.</summary>
    private static readonly TimeSpan InitialSweepDelay = TimeSpan.FromSeconds(10);

    // ── Dependencies ─────────────────────────────────────────────────────
    private readonly IServiceScopeFactory          _scopeFactory;
    private readonly ISmsGatewaySettingsCache      _cache;
    //private readonly IMediator                     _mediator;
    private readonly ILogger<SmsInboundService>    _log;

    // ── Runtime state ────────────────────────────────────────────────────
    private readonly SemaphoreSlim          _wake     = new(0, 1);   // settings-change signal
    private readonly SemaphoreSlim          _pollGate = new(1, 1);   // single-flight
    private readonly Dictionary<long, int>  _poisonCounters = new();

    private long _lastId;
    private bool _cursorLoaded;
    private int  _consecutiveFailures;

    /// <summary>
    /// The settings snapshot we last acted on. When the cache's reference
    /// changes, both phases are forced to re-evaluate immediately.
    /// </summary>
    private SmsGatewaySettingsDto? _lastCfg;



    public SmsInboundService(
    IServiceScopeFactory scopeFactory,
    ISmsGatewaySettingsCache cache,
    ILogger<SmsInboundService> log)
{
    _scopeFactory = scopeFactory;
    _cache        = cache;
    _log          = log;
}

/// <summary>
/// Runs <paramref name="work"/> against a fresh scoped
/// <see cref="IApplicationDbContext"/>. The scope is disposed on return.
/// This is the sanctioned way for a singleton hosted service to touch
/// scoped services like the DbContext factory.
/// </summary>
private async Task<T> WithDbAsync<T>(
    Func<IApplicationDbContext, Task<T>> work, CancellationToken ct)
{
    using var scope = _scopeFactory.CreateScope();
    var factory = scope.ServiceProvider
        .GetRequiredService<IApplicationDbContextFactory>();
    await using var db = await factory.CreateAsync(ct).ConfigureAwait(false);
    return await work(db).ConfigureAwait(false);
}

private async Task<T> WithMediatorAsync<T>(
    Func<IMediator, ValueTask<T>> work, CancellationToken ct)
{
    using var scope = _scopeFactory.CreateScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    return await work(mediator).ConfigureAwait(false);
}
    // ─────────────────────────────────────────────────────────────────────
    // Main loop
    // ─────────────────────────────────────────────────────────────────────

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _log.LogInformation("SMS inbound service started.");

        _cache.Changed += OnSettingsChanged;

        await EnsureCacheLoadedAsync(stoppingToken).ConfigureAwait(false);
        await TryLoadCursorAsync(stoppingToken).ConfigureAwait(false);

        var nextPollAt  = DateTime.UtcNow;
        var nextSweepAt = DateTime.UtcNow + InitialSweepDelay;

        while (!stoppingToken.IsCancellationRequested)
        {
            var cfg = _cache.Current;
            var now = DateTime.UtcNow;

            // ── Settings snapshot changed → force both phases due. ───────
            if (!ReferenceEquals(cfg, _lastCfg))
            {
                _lastCfg    = cfg;
                nextPollAt  = now;
                nextSweepAt = now;
            }

            if (cfg is null)
            {
                await SafeWaitAsync(TimeSpan.FromSeconds(10), stoppingToken)
                    .ConfigureAwait(false);
                continue;
            }

            // ── Master gate: polling off → nothing runs. ─────────────────
            if (!cfg.EnableInboundPolling)
            {
                await SafeWaitAsync(PollingDisabledRecheck, stoppingToken)
                    .ConfigureAwait(false);
                continue;
            }

            // ── Poll phase ───────────────────────────────────────────────
            if (now >= nextPollAt)
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

                    var interval = TimeSpan.FromSeconds(
                        Math.Max(5, cfg.PollingIntervalSeconds));
                    nextPollAt = DateTime.UtcNow + interval;
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

                    nextPollAt = DateTime.UtcNow + backoff;
                }
            }

            // ── Sweep phase (only reached when polling is enabled). ──────
            if (now >= nextSweepAt)
            {
                nextSweepAt = DateTime.UtcNow + TimeSpan.FromSeconds(
                    Math.Max(10, cfg.SweeperIntervalSeconds));

                try
                {
                    await RunSweeperAsync(cfg, stoppingToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "SMS sweeper iteration failed.");
                }
            }

            // ── Wait until the earlier of the two next-due times. ────────
            var nextWake = nextPollAt < nextSweepAt ? nextPollAt : nextSweepAt;
            var delay    = nextWake - DateTime.UtcNow;
            if (delay < TimeSpan.Zero) delay = TimeSpan.Zero;

            await SafeWaitAsync(delay, stoppingToken).ConfigureAwait(false);
        }

        _cache.Changed -= OnSettingsChanged;
        _log.LogInformation("SMS inbound service stopped.");
    }

    // ─────────────────────────────────────────────────────────────────────
    // Settings / cache / cursor
    // ─────────────────────────────────────────────────────────────────────

    private void OnSettingsChanged(object? sender, SmsGatewaySettingsDto e)
    {
        try { _wake.Release(); }
        catch (SemaphoreFullException) { /* already pending — fine */ }
    }

    private async Task EnsureCacheLoadedAsync(CancellationToken ct)
    {
        if (_cache.Current is not null) return;
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var provider = scope.ServiceProvider
                .GetRequiredService<ISmsGatewaySettingsProvider>();
            await provider.GetAsync(ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex,
                "Could not preload SMS gateway settings; will retry on next iteration.");
        }
    }

    /// <summary>
    /// Blocks until <paramref name="delay"/> elapses, or the wake semaphore
    /// fires (settings change), or <paramref name="ct"/> is cancelled.
    /// Drains the wake signal if it fired so the next wait behaves correctly.
    /// </summary>
    private async Task SafeWaitAsync(TimeSpan delay, CancellationToken ct)
    {
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var wake  = _wake.WaitAsync(linked.Token);
        var sleep = Task.Delay(delay, linked.Token);

        await Task.WhenAny(wake, sleep).ConfigureAwait(false);
        linked.Cancel();

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

    // ─────────────────────────────────────────────────────────────────────
    // Backoff / diagnostics
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Exponential backoff with ±20% jitter, capped at <see cref="MaxBackoffSeconds"/>.
    /// 5s → 10s → 20s → 40s → 60s → 60s…
    /// </summary>
    private static TimeSpan ComputeBackoff(int failures)
    {
        var exponent    = Math.Min(failures - 1, 4);
        var baseSeconds = Math.Min(MaxBackoffSeconds, 5 * Math.Pow(2, exponent));
        var jitter      = 0.8 + Random.Shared.NextDouble() * 0.4;
        return TimeSpan.FromSeconds(baseSeconds * jitter);
    }

    private static string ClassifyReason(Exception ex) => ex switch
    {
        SmsGatewayException { StatusCode: not null } s =>
            $"gateway op={s.Operation} status={s.StatusCode}",
        SmsGatewayException s =>
            $"gateway op={s.Operation} (no status)",
        HttpRequestException http =>
            $"transport status={(int?)http.StatusCode}",
        TaskCanceledException      => "http timeout",
        OperationCanceledException => "cancelled",
        _ => ex.GetType().Name
    };

    private int RecordPoisonFailure(long gatewayId)
    {
        var next = (_poisonCounters.TryGetValue(gatewayId, out var n) ? n : 0) + 1;
        _poisonCounters[gatewayId] = next;
        return next;
    }

    // ─────────────────────────────────────────────────────────────────────
    // Poll phase
    // ─────────────────────────────────────────────────────────────────────

    private async Task PollOnceAsync(CancellationToken ct)
    {
        // Single-flight guard: skip this tick if a poll is already running.
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
                    // Multipart fields are forwarded so the poller feeds the
                    // same assembly path as the webhook.
                    var result = await mediator.Send(
                        new PersistInboundSmsCommand(new InboundSmsDto(
                            GatewayId:  item.Id,
                            From:       item.From,
                            Body:       item.Body,
                            Ts:         item.Ts,
                            SimSlot:    item.SimSlot,
                            ConcatRef:  item.ConcatRef,
                            PartNumber: item.PartNumber,
                            TotalParts: item.TotalParts)),
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

                        if (RecordPoisonFailure(item.Id) >= MaxPoisonRetries)
                        {
                            _log.LogError(
                                "Gateway ID {Id} failed {N} times — skipping permanently.",
                                item.Id, MaxPoisonRetries);
                            if (item.Id > maxSeen) maxSeen = item.Id;
                            skipped++;
                            _poisonCounters.Remove(item.Id);
                        }
                        else
                        {
                            break; // retry next poll from the same cursor
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

    // ─────────────────────────────────────────────────────────────────────
    // Sweep phase
    // ─────────────────────────────────────────────────────────────────────

    private async Task RunSweeperAsync(
        SmsGatewaySettingsDto cfg, CancellationToken ct)
    {
        var assemblyWindow  = TimeSpan.FromMinutes(Math.Max(1, cfg.AssemblyWindowMinutes));
        var orphanThreshold = TimeSpan.FromMinutes(Math.Max(1, cfg.OrphanThresholdMinutes));
        var retention       = TimeSpan.FromDays(Math.Max(1, cfg.RetentionDays));
        var maxGroups       = Math.Clamp(cfg.MaxGroupsPerSweep, 1, 1000);

        await AssembleStrandedGroupsAsync(assemblyWindow, maxGroups, ct)
            .ConfigureAwait(false);
        await DiscardOrphansAsync(orphanThreshold, ct)
            .ConfigureAwait(false);
        await PruneAssembledPartsAsync(retention, ct)
            .ConfigureAwait(false);
    }

    // ── Sweep 1: assemble stranded groups ────────────────────────────────

private async Task AssembleStrandedGroupsAsync(
    TimeSpan assemblyWindow, int maxGroups, CancellationToken ct)
{
    var cutoff = DateTime.UtcNow - assemblyWindow;

    // ① Read the stranded keys in one short-lived scope.
    var strandedKeys = await WithDbAsync(async db =>
        await db.SmsInboundParts
            .Where(p => p.AssembledSmsMessageId == null && p.CreatedAt >= cutoff)
            .GroupBy(p => new { p.From, p.ConcatRef, p.TotalParts })
            .Where(g => g.Count() >= g.Key.TotalParts)
            .Select(g => new { g.Key.From, g.Key.ConcatRef, g.Key.TotalParts })
            .Take(maxGroups)
            .ToListAsync(ct)
            .ConfigureAwait(false),
        ct).ConfigureAwait(false);

    if (strandedKeys.Count == 0) return;

    _log.LogInformation(
        "Sweeper found {N} stranded multipart group(s) to retry.",
        strandedKeys.Count);

    foreach (var k in strandedKeys)
    {
        if (ct.IsCancellationRequested) break;

        // ② Read the seed part in its own scope — one per iteration, so we
        //    never hold a DbContext across the mediator call below.
        var seed = await WithDbAsync(async db =>
            await db.SmsInboundParts
                .Where(p => p.From       == k.From &&
                            p.ConcatRef  == k.ConcatRef &&
                            p.TotalParts == k.TotalParts &&
                            p.AssembledSmsMessageId == null &&
                            p.CreatedAt  >= cutoff)
                .OrderByDescending(p => p.PartNumber)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false),
            ct).ConfigureAwait(false);

        if (seed is null) continue;

        var dto = new InboundSmsDto(
            GatewayId:  seed.GatewayId,
            From:       seed.From,
            Body:       seed.Body,
            Ts:         seed.Ts,
            SimSlot:    seed.SimSlot,
            ConcatRef:  seed.ConcatRef,
            PartNumber: seed.PartNumber,
            TotalParts: seed.TotalParts);

        try
        {

            var result = await WithMediatorAsync(
    m => m.Send(new PersistInboundSmsCommand(dto), ct),
    ct).ConfigureAwait(false);

            // var result = await _mediator
            //     .Send(new PersistInboundSmsCommand(dto), ct)
            //     .ConfigureAwait(false);

            if (result.Succeeded && result.Data)
            {
                _log.LogWarning(
                    "Sweeper assembled stranded multipart ref={Ref} from={From} parts={N}.",
                    k.ConcatRef, k.From, k.TotalParts);
            }
            else if (!result.Succeeded)
            {
                var errs = string.Join("; ", result.Errors ?? new List<string>());
                _log.LogWarning(
                    "Sweeper could not assemble ref={Ref} from={From}: {Errors}.",
                    k.ConcatRef, k.From, errs);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            break;
        }
        catch (Exception ex)
        {
            _log.LogError(ex,
                "Sweeper threw while assembling ref={Ref} from={From}.",
                k.ConcatRef, k.From);
        }
    }
}

    // ── Sweep 2: discard orphans ────────────────────────────────────────


private async Task DiscardOrphansAsync(
    TimeSpan orphanThreshold, CancellationToken ct)
{
    var cutoff = DateTime.UtcNow - orphanThreshold;

    var deleted = await WithDbAsync(async db =>
        await db.SmsInboundParts
            .Where(p => p.AssembledSmsMessageId == null && p.CreatedAt < cutoff)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false),
        ct).ConfigureAwait(false);

    if (deleted > 0)
    {
        _log.LogWarning(
            "Discarded {N} orphaned SMS part(s) older than {Minutes} min.",
            deleted, orphanThreshold.TotalMinutes);
    }
}

    // ── Sweep 3: prune assembled parts ──────────────────────────────────


private async Task PruneAssembledPartsAsync(
    TimeSpan retention, CancellationToken ct)
{
    var cutoff = DateTime.UtcNow - retention;

    var deleted = await WithDbAsync(async db =>
        await db.SmsInboundParts
            .Where(p => p.AssembledSmsMessageId != null && p.CreatedAt < cutoff)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false),
        ct).ConfigureAwait(false);

    if (deleted > 0)
    {
        _log.LogInformation(
            "Pruned {N} assembled SMS part(s) older than {Days} day(s).",
            deleted, retention.TotalDays);
    }
}
  


}