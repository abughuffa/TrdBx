////using System.Text.RegularExpressions;

////namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks;
////public partial class SharedLogic
////{

////    public async Task<string> GenSerialNo(IApplicationDbContext _context, string SerialFor, DateOnly? date)
////    {
////        var now = date is null ? DateOnly.FromDateTime(DateTime.Now) : date;
////        var prefix = $"{now:yyyyMM}-";
////        var sequenceNumber = 1;
////        var serialNo = string.Empty;

////        switch (SerialFor)
////        {
////            case "Asset":
////                {
////                    // Get latest asset number for current month
////                    var lastAsset = await _context.Assets.Where(i => i.AssetNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.AssetNo).FirstOrDefaultAsync();
////                    if (lastAsset != null)
////                    {
////                        var match = Regex.Match(lastAsset.AssetNo, @$"^{prefix}(\d+)$");
////                        if (match.Success && int.TryParse(match.Groups[1].Value, out int lastSequence))
////                        {
////                            sequenceNumber = lastSequence + 1;
////                        }
////                    }
////                    serialNo = $"{prefix}{sequenceNumber:D3}";
////                    break;
////                }

////            case "ServiceLog":
////                {

////                    // Get latest serviceLog number for current month
////                    var lastserviceLog = await _context.ServiceLogs.Where(i => i.ServiceNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.ServiceNo).FirstOrDefaultAsync();
////                    if (lastserviceLog != null)
////                    {
////                        var match = Regex.Match(lastserviceLog.ServiceNo, @$"^{prefix}(\d+)$");
////                        if (match.Success && int.TryParse(match.Groups[1].Value, out int lastSequence))
////                        {
////                            sequenceNumber = lastSequence + 1;
////                        }
////                    }
////                    serialNo = $"{prefix}{sequenceNumber:D3}";
////                    break;
////                }

////            case "Invoice":
////                {
////                    // Get latest invoice number for current month
////                    var lastInvoice = await _context.Invoices.Where(i => i.InvNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.InvNo).FirstOrDefaultAsync();
////                    if (lastInvoice != null)
////                    {
////                        var match = Regex.Match(lastInvoice.InvNo, @$"^{prefix}(\d+)$");
////                        if (match.Success && int.TryParse(match.Groups[1].Value, out int lastSequence))
////                        {
////                            sequenceNumber = lastSequence + 1;
////                        }
////                    }
////                    serialNo = $"{prefix}{sequenceNumber:D3}";
////                    break;
////                }
////            case "Ticket":
////                {
////                    // Get latest ticket number for current month
////                    var lastTicket = await _context.Tickets.Where(i => i.TicketNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.TicketNo).FirstOrDefaultAsync();
////                    if (lastTicket != null)
////                    {
////                        var match = Regex.Match(lastTicket.TicketNo, @$"^{prefix}(\d+)$");
////                        if (match.Success && int.TryParse(match.Groups[1].Value, out int lastSequence))
////                        {
////                            sequenceNumber = lastSequence + 1;
////                        }
////                    }
////                    serialNo = $"{prefix}{sequenceNumber:D3}";
////                    break;
////                }
                

////            default:
////                {
////                    throw new NotImplementedException($"Couldn't create serial number for Object {SerialFor}, which passed!");
////                }
////        }
////        return serialNo;
////    }




////}
