using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using DocumentFormat.OpenXml.Wordprocessing;


namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.RenewSubscription;

public class RenewSubscriptionCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]  public int Id { get; }
    public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Description("CreateAnnualSub")] public bool CreateAnnualSub { get; set; } = false;
}
public class RenewSubscriptionCommandHandler : PriceSharedLogic, IRequestHandler<RenewSubscriptionCommand, Result<int>>
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
    private readonly IStringLocalizer<RenewSubscriptionCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public RenewSubscriptionCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<RenewSubscriptionCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(RenewSubscriptionCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).Include(u => u.TrackedAsset).Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync(cancellationToken);

        if (!(unit.UStatus == UStatus.InstalledActiveHosting || unit.UStatus == UStatus.InstalledActiveGprs || unit.UStatus == UStatus.InstalledActive))
        {
            return await Result<int>.FailureAsync(_localizer["StatusControlException"]);
        }

        if (!(unit.Subscriptions?.OrderBy(x => x.Id).LastOrDefault().SeDate < DateOnly.FromDateTime(new DateTime(request.TsDate.Year, 12, 31))))
        {
            //throw new Exception("Tracking Unit Subscription End date should be less than current period end date to Renew it.");
            return await Result<int>.FailureAsync("Selected unit Subscription greater than current period end date");
        }

        var dailyFees = 0.0m;
        var OLF = SubPackageFees.ZeroFees;

        var price = await GetCPrice(_context,(int)unit.CustomerId, unit.TrackingUnitModelId);

        var serviceNo = await GenSerialNo(_context, "ServiceLog", request.TsDate);

        var currentSubscription = unit.Subscriptions?.OrderBy(x => x.Id).LastOrDefault();

            var startDate = currentSubscription.SeDate.AddDays(1);

            var endDate = request.CreateAnnualSub == true? currentSubscription.SeDate.AddDays(366) : DateOnly.FromDateTime(new DateTime(request.TsDate.Year, 12, 31));

            var days = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days;

            var caseCode = request.CreateAnnualSub == true ? 1 : 0;

            if (endDate > currentSubscription.SeDate)
            {
                var serviceLog = new ServiceLog()
                {
                    Description = $"تجديد اشتراك الوحدة ({unit.SNo}) بالمركبة ({unit.UnitName}).",
                    ServiceNo = serviceNo,
                    ServiceTask = ServiceTask.RenewUnitSub,
                    CustomerId = (int)unit.CustomerId,
                    SerDate = request.TsDate,
                    Amount = 0.0m,
                    IsDeserved = true,
                    IsBilled = false,
                };

                switch (unit.UStatus)
                {
                    case UStatus.InstalledActiveGprs:
                        {
                            OLF = SubPackageFees.GprsFees;
                            dailyFees = Math.Round(price.Gprs / 365, 3, MidpointRounding.AwayFromZero);
                            serviceLog.Subscriptions = [ new() {
                                    CaseCode = caseCode,
                                    LastPaidFees = OLF,
                                    Description = string.Format($"دورة اشتراك تمديد جديدة من التاريخ {startDate} حتى التاريخ {endDate}."),
                                    TrackingUnitId = unit.Id,
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
                                    Description = string.Format($"دورة اشتراك استضافة جديدة من التاريخ {startDate} حتى التاريخ {endDate}."),
                                    TrackingUnitId = unit.Id,
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
                                    Description = string.Format($"دورة اشتراك كامل جديدة من التاريخ {startDate} حتى التاريخ {endDate}."),
                                    TrackingUnitId = unit.Id,
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
                                    Description = string.Format($"دورة اشتراك صفرية"),
                                    TrackingUnitId = unit.Id,
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
            


        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return await Result<int>.SuccessAsync(result);
        }
        else
            return await Result<int>.FailureAsync(_localizer["Renew Subscription Faild"]);



    }
    else
    return await Result<int>.FailureAsync(_localizer["Renew Subscription Faild"]);
}
}

