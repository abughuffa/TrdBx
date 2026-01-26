using System.Text.RegularExpressions;
using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Renew;

public class RenewTrackingUnitSubscriptionCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]  public int[] Id { get; }
    public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    public RenewTrackingUnitSubscriptionCommand(int[] id)
    {
        Id = id;
    }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Description("CreateAnnualSub")] public bool CreateAnnualSub { get; set; } = false;
    //[Description("InstallerId")] public string InstallerId { get; set; } = string.Empty;
}
public class RenewTrackingUnitSubscriptionCommandHandler : PriceSharedLogic, IRequestHandler<RenewTrackingUnitSubscriptionCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<RenewTrackingUnitSubscriptionCommandHandler> _localizer;
    //private readonly IWialonService _wialonService;
    //public RenewTrackingUnitSubscriptionCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<RenewTrackingUnitSubscriptionCommandHandler> localizer,
    //                                     IWialonService wialonService)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _wialonService = wialonService;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<RenewTrackingUnitSubscriptionCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public RenewTrackingUnitSubscriptionCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<RenewTrackingUnitSubscriptionCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(RenewTrackingUnitSubscriptionCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var units = await _context.TrackingUnits.Where(x => request.Id.Contains(x.Id)).Include(u => u.TrackedAsset).Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).ToListAsync(cancellationToken);

        if (!units.Any(u => u.UStatus == UStatus.InstalledActiveHosting || u.UStatus == UStatus.InstalledActiveGprs || u.UStatus == UStatus.InstalledActive))
        {
            return await Result<int>.FailureAsync(_localizer["StatusControlException"]);
        }

        var items = units.Where(u => u.Subscriptions?.OrderBy(x => x.Id).LastOrDefault().SeDate < DateOnly.FromDateTime(new DateTime(request.TsDate.Year, 12, 31)));

        if (!items.Any())
        {
            //throw new Exception("Tracking Unit Subscription End date should be less than current period end date to Renew it.");
            return await Result<int>.FailureAsync("All selected units Subscriptions greater than current period end date");
        }

        var dailyFees = 0.0m;
        var OLF = SubPackageFees.ZeroFees;

        var prefix = $"{request.TsDate:yyyyMM}-";
        var sequenceNumber = 1;
        var serialNo = string.Empty;
        var lastserviceLog = await _context.ServiceLogs.Where(i => i.ServiceNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.ServiceNo).FirstOrDefaultAsync();
        if (lastserviceLog != null)
            {
                var match = Regex.Match(lastserviceLog.ServiceNo, @$"^{prefix}(\d+)$");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int lastSequence))
                {
                    sequenceNumber = lastSequence +1;
                }
            }


        foreach (var item in items)
        { 
            var xserviceNo  = $"{prefix}{sequenceNumber:D3}";

            var price = await GetCPrice(_context, (int)item.CustomerId, item.TrackingUnitModelId);

            var currentSubscription = item.Subscriptions?.OrderBy(x => x.Id).LastOrDefault();

            var startDate = currentSubscription.SeDate.AddDays(1);

            var endDate = request.CreateAnnualSub == true? currentSubscription.SeDate.AddDays(366) : DateOnly.FromDateTime(new DateTime(request.TsDate.Year, 12, 31));

            var days = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days;

            var caseCode = request.CreateAnnualSub == true ? 1 : 0;

            if (endDate > currentSubscription.SeDate)
            {
                var serviceLog = new ServiceLog()
                {
                    Desc = $"تجديد اشتراك الوحدة ({item.SNo}) بالمركبة ({item.UnitName}).",
                    //Desc = string.Format("تجديد اشتراك الوحدة ({0}).", item.SNo),
                    ServiceNo = xserviceNo,
                    ServiceTask = ServiceTask.RenewUnitSub,
                    CustomerId = (int)item.CustomerId,
                    //InstallerId = request.InstallerId,
                    SerDate = request.TsDate,
                    Amount = 0.0m,
                    IsDeserved = true,
                    IsBilled = false,
                };

                switch (item.UStatus)
                {
                    case UStatus.InstalledActiveGprs:
                        {
                            OLF = SubPackageFees.GprsFees;
                            dailyFees = Math.Round(price.Gprs / 365, 3, MidpointRounding.AwayFromZero);
                            serviceLog.Subscriptions = [ new() {
                                    CaseCode = caseCode,
                                    LastPaidFees = OLF,
                                    Desc = string.Format($"دورة اشتراك تمديد جديدة من التاريخ {startDate} حتى التاريخ {endDate}."),
                                    TrackingUnitId = item.Id,
                                    SsDate = startDate,
                                    SeDate = endDate,
                                    DailyFees = dailyFees,
                                    //Days = days,
                                    //Amount = Math.Round(days * dailyFees, 3, MidpointRounding.AwayFromZero),
                                }];
                            serviceLog.WialonTasks = [];
                            break;
                        }
                    case UStatus.InstalledActiveHosting:
                        {
                            OLF = SubPackageFees.HostFees;
                            dailyFees = Math.Round(price.Host / 365, 3, MidpointRounding.AwayFromZero);
                            serviceLog.Subscriptions = [ new() {
                                    LastPaidFees = OLF,
                                    //SubPackageFees = SubPackageFees.GprsFees,
                                    Desc = string.Format($"دورة اشتراك استضافة جديدة من التاريخ {startDate} حتى التاريخ {endDate}."),
                                    TrackingUnitId = item.Id,
                                    SsDate = startDate,
                                    SeDate = endDate,
                                    DailyFees = dailyFees,
                                    //Days = days,
                                    //Amount = Math.Round(days * dailyFees, 3, MidpointRounding.AwayFromZero),
                                }];
                            serviceLog.WialonTasks = [];
                            break;
                        }
                    case UStatus.InstalledActive:
                        {
                            OLF = SubPackageFees.FullFees;
                            dailyFees = Math.Round((price.Gprs + price.Host) / 365, 3, MidpointRounding.AwayFromZero);
                            serviceLog.Subscriptions = [ new() {
                                    LastPaidFees = OLF,
                                    //SubPackageFees = SubPackageFees.GprsFees,
                                    Desc = string.Format($"دورة اشتراك كامل جديدة من التاريخ {startDate} حتى التاريخ {endDate}."),
                                    TrackingUnitId = item.Id,
                                    SsDate = startDate,
                                    SeDate = endDate,
                                    DailyFees = dailyFees,
                                    //Days = days,
                                    //Amount = Math.Round(days * dailyFees, 3, MidpointRounding.AwayFromZero),
                                }];
                            serviceLog.WialonTasks = [];
                            break;
                        }
                    default:
                        {
                            OLF = SubPackageFees.ZeroFees;
                            dailyFees = 0.0m;
                            serviceLog.Subscriptions = [ new() {
                                    LastPaidFees = SubPackageFees.ZeroFees,
                                    Desc = string.Format($"دورة اشتراك صفرية"),
                                    TrackingUnitId = item.Id,
                                    SsDate = startDate,
                                    SeDate = startDate,
                                    DailyFees = 0.0m,
                                    //Days = 0,
                                    //Amount = 0.0m,
                                }];
                            serviceLog.WialonTasks = [];
                            break;
                        }
                }

                serviceLog.AddDomainEvent(new ServiceLogCreatedEvent(serviceLog));

                _context.ServiceLogs.Add(serviceLog);

                sequenceNumber++;

            }
        }

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return await Result<int>.SuccessAsync(result);
        }
        else
            return await Result<int>.FailureAsync(_localizer["RenewTrackingUnitSubscriptionFaild"]);



    }
}

