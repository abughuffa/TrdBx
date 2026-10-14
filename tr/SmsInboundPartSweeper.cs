// using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
// using CleanArchitecture.Blazor.Domain.Events;

// namespace CleanArchitecture.Blazor.Server.UI.Services;

// /// <summary>
// /// Two responsibilities:
// ///   1. Assemble any complete group that the fast path missed due to a race
// ///      (defensive — should be rare).
// ///   2. Discard orphaned parts whose siblings never arrived.
// /// </summary>
// public sealed class SmsInboundPartSweeper : BackgroundService
// {
//     private static readonly TimeSpan AssemblyWindow  = TimeSpan.FromMinutes(5);
//     private static readonly TimeSpan OrphanThreshold = TimeSpan.FromMinutes(20);
//     private static readonly TimeSpan Interval        = TimeSpan.FromMinutes(1);

//     private readonly IApplicationHubWrapper _applicationHubWrapper;

//     private readonly IApplicationDbContextFactory _dbContextFactory;
//     private readonly ILogger<SmsInboundPartSweeper> _log;

//     public SmsInboundPartSweeper(
//         IApplicationDbContextFactory dbContextFactory,
//         IApplicationHubWrapper applicationHubWrapper,
//         ILogger<SmsInboundPartSweeper> log)
//     {
//         _applicationHubWrapper = applicationHubWrapper;
//         _dbContextFactory = dbContextFactory;
//         _log          = log;
//     }

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         _log.LogInformation("SMS part sweeper started.");

//         while (!stoppingToken.IsCancellationRequested)
//         {
//             try
//             {
//                 // using var scope = _scopeFactory.CreateScope();
//                 // var _dbContextFactory = scope.ServiceProvider.GetRequiredService<IApplicationDbContextFactory>();
//                 //var hub = scope.ServiceProvider.GetRequiredService<IApplicationHubWrapper>();

//                 await using var context = await _dbContextFactory.CreateAsync(stoppingToken);

//                 await AssembleCompleteGroupsAsync(context, _applicationHubWrapper, stoppingToken);
//                 await DiscardOrphansAsync(context, stoppingToken);
//             }
//             catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }
//             catch (Exception ex)
//             {
//                 _log.LogError(ex, "SMS part sweeper iteration failed.");
//             }

//             try { await Task.Delay(Interval, stoppingToken); }
//             catch (OperationCanceledException) { break; }
//         }

//         _log.LogInformation("SMS part sweeper stopped.");
//     }

//     // ------------------------------------------------------------------

//     private async Task AssembleCompleteGroupsAsync(
//         IApplicationDbContext db,
//         IApplicationHubWrapper hub,
//         CancellationToken ct)
//     {
//         var cutoff = DateTime.UtcNow - AssemblyWindow;

//         var groups = await db.SmsInboundParts
//             .Where(p => p.AssembledSmsMessageId == null && p.CreatedAt >= cutoff)
//             .GroupBy(p => new { p.From, p.ConcatRef, p.TotalParts })
//             .Where(g => g.Count() >= g.Key.TotalParts)
//             .Select(g => new { g.Key.From, g.Key.ConcatRef, g.Key.TotalParts })
//             .ToListAsync(ct);

//         foreach (var g in groups)
//         {
//             if (ct.IsCancellationRequested) break;

//             var parts = await db.SmsInboundParts
//                 .Where(p => p.From == g.From
//                          && p.ConcatRef == g.ConcatRef
//                          && p.AssembledSmsMessageId == null)
//                 .OrderBy(p => p.PartNumber)
//                 .ToListAsync(ct);

//             if (parts.Count < g.TotalParts) continue;   // raced with the handler

//             var body = string.Concat(parts.Select(p => p.Body));
//             var first = parts[0];

//             var entity = new SmsMessage
//             {
//                 Direction  = Domain.Enums.SmsDirection.Inbound,
//                 From       = first.From,
//                 To         = "self",
//                 Body       = body,
//                 GatewayId  = first.GatewayId.ToString(),
//                 SimSlot    = first.SimSlot,
//                 ReceivedAt = DateTimeOffset.FromUnixTimeMilliseconds(first.Ts).UtcDateTime,
//                 Status     = Domain.Enums.SmsStatus.Delivered,
//             };
//             entity.AddDomainEvent(new SmsMessageCreatedEvent(entity));

//             try
//             {
//                 db.SmsMessages.Add(entity);
//                 await db.SaveChangesAsync(ct);

//                 foreach (var p in parts) p.AssembledSmsMessageId = entity.Id;
//                 await db.SaveChangesAsync(ct);
//             }
//             catch (DbUpdateException)
//             {
//                 // Lost a race with the handler or another sweeper instance.
//                 continue;
//             }

//             _log.LogWarning(
//                 "Sweeper assembled stray multipart SMS: ref={Ref} from={From} parts={N}",
//                 g.ConcatRef, g.From, parts.Count);

//             await hub.SmsReceived(new SmsReceivedPayload(
//                 Id:         entity.Id,
//                 GatewayId:  first.GatewayId,
//                 From:       first.From,
//                 Body:       body,
//                 Ts:         first.Ts,
//                 ReceivedAt: (DateTime)entity.ReceivedAt));
//         }
//     }

//     private async Task DiscardOrphansAsync(
//         IApplicationDbContext db, CancellationToken ct)
//     {
//         var cutoff = DateTime.UtcNow - OrphanThreshold;

//         var orphans = await db.SmsInboundParts
//             .Where(p => p.AssembledSmsMessageId == null && p.CreatedAt < cutoff)
//             .ToListAsync(ct);

//         if (orphans.Count == 0) return;

//         _log.LogWarning(
//             "Discarding {N} orphaned SMS parts (groups: {Groups}).",
//             orphans.Count,
//             string.Join(",", orphans.Select(p => $"{p.From}/{p.ConcatRef}").Distinct()));

//         db.SmsInboundParts.RemoveRange(orphans);
//         await db.SaveChangesAsync(ct);
//     }
// }