using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Replace;

public class XReplaceGpsUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Description("SUnitId")] public int SUnitId { get; set; }
    [Description("SimCardId")] public int SimCardId { get; set; } = 0;
    [Description("CustomerId")] public int CustomerId { get; set; }
    [Description("InstallerId")] public string InstallerId { get; set; } = string.Empty;
    [Description("SubPackage")] public SubPackage SubPackage { get; set; } = SubPackage.Active;
    [Description("InsMode")] public InsMode InsMode { get; set; }
    [Description("CreateDeservedServices")] public bool CreateDeservedServices { get; set; }
    [Description("IsTampred")] public bool IsTampred { get; set; }
    [Description("ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;


}

public class XReplaceGpsUnitCommandHandler : SubscriptionSharedLogic, IRequestHandler<XReplaceGpsUnitCommand, Result<int>>
{



    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<XReplaceGpsUnitCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public XReplaceGpsUnitCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<XReplaceGpsUnitCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }



    public async Task<Result<int>> Handle(XReplaceGpsUnitCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var runit = await _context.TrackingUnits.Where(x => x.Id == request.Id).Include(x => x.Subscriptions).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(runit.UStatus == UStatus.InstalledActive || runit.UStatus == UStatus.InstalledActiveHosting || runit.UStatus == UStatus.InstalledActiveGprs || runit.UStatus == UStatus.InstalledInactive))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be Installed to procced");
        }

        var sunit = await _context.TrackingUnits.Where(x => x.Id == request.SUnitId).Include(x => x.Subscriptions).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.SUnitId}] not found.");

        if (!(sunit.UStatus == UStatus.New || sunit.UStatus == UStatus.Reserved || sunit.UStatus == UStatus.Used))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be New/Reserved or used to procced");
        }

        if (sunit.UStatus == UStatus.New || sunit.UStatus == UStatus.Reserved)
        {
            sunit.Subscriptions = new List<Subscription>();
        }

        sunit.CustomerId = request.CustomerId;

        var sim = await _context.SimCards.FindAsync(new object[] { request.SimCardId }, cancellationToken) ?? throw new NotFoundException($"SimCard with id: [{request.SimCardId}] not found.");

#pragma warning disable CS8601 // Possible null reference assignment.
        var asset = await _context.TrackedAssets.FindAsync(new object[] { runit.TrackedAssetId }, cancellationToken) ?? throw new NotFoundException($"TrackedAsset with id: [{runit.TrackedAssetId}] not found.");
#pragma warning restore CS8601 // Possible null reference assignment.

#pragma warning disable CS8629 // Nullable value type may be null.
        var rprice = GetCPrice(_context,  (int)runit.CustomerId, runit.TrackingUnitModelId);
#pragma warning restore CS8629 // Nullable value type may be null.

        var sprice = GetCPrice(_context,  (int)sunit.CustomerId, sunit.TrackingUnitModelId);

        var T = request.IsTampred;  //IsTampred
        var R = runit.WryDate < request.TsDate; //Replaced Unit Warrenty
        var S = sunit.WryDate is null ? true : sunit.WryDate <= request.TsDate ? true : false; //Selected Unit Warrenty

        var IsObserved = !R || S;
        var Rw = R && S;
        var Sw = !T && R && S;

        var serviceNo = GenSerialNo(_context, "ServiceLog", request.TsDate).Result;

        var serviceLog = new ServiceLog()
        {
            ServiceNo = serviceNo,
            ServiceTask = ServiceTask.Replace,
            CustomerId = request.CustomerId,
            InstallerId = request.InstallerId,
            SerDate = request.TsDate,
            IsDeserved = IsObserved,
            IsBilled = false,
            Subscriptions = new List<Subscription>(),
            WialonTasks = new List<WialonTask>()
        };

        switch (sunit.UStatus)
        {
            case UStatus.Used:
                {
                    serviceLog.Desc = string.Format("استبدال الوحدة ({0}) بالوحدة المستعملة ({1}) للأصل ({2})", runit.SNo, sunit.SNo, asset.TrackedAssetNo);
                    serviceLog.Amount = GetSPrice(_context, ServiceTask.Replace);

                    sunit.WryDate = Sw ? request.TsDate : sunit.WryDate;

                    if (sunit.SimCardId != null && sim.Id != sunit.SimCardId)
                    {
                        var oldSimCard = _context.SimCards.Where(a => a.Id == (int)sunit.SimCardId).FirstOrDefault();
                        oldSimCard.SStatus = SStatus.Recovered; //Set as Recovered
                        oldSimCard.AddDomainEvent(new SimCardUpdatedEvent(oldSimCard));
                    }

                    break;
                }
            case UStatus.Reserved:
            case UStatus.New:
                {
                    serviceLog.Desc = string.Format("استبدال الوحدة ({0}) بالوحدة الجديدة ({1}) للأصل ({2})", runit.SNo, sunit.SNo, asset.TrackedAssetNo);
                    sunit.WryDate = Sw ? request.TsDate : request.TsDate.AddDays(365);

                    serviceLog.Amount = sprice.Price;
                    break;
                }
        }

        if (runit.SimCardId != null && sim.Id == runit.SimCardId)
        {
            runit.SimCardId = null;
        }

        {
            if (runit.UStatus == UStatus.InstalledActive || runit.UStatus == UStatus.InstalledActiveHosting || runit.UStatus == UStatus.InstalledActiveGprs)
            {
                Deactivate(runit, serviceLog, request.TsDate, rprice, true);
            }
            else if (runit.UStatus == UStatus.InstalledInactive && runit.IsOnWialon)
            {
                serviceLog.WialonTasks.Add(new WialonTask()
                {
                    TrackingUnitId = runit.Id,
                    APITask = APITask.RemoveFromWialon,
                    Desc = string.Format("حذف الوحدة ({0}) من منصة ويلون.", runit.SNo),
                    ExcDate = request.TsDate,
                    IsExecuted = false,
                });
            }

            runit.UnitName = null;
            runit.UStatus = UStatus.Recovered;
            runit.TrackedAssetId = null;
            runit.InsMode = InsMode.Null;
            runit.WryDate = Rw ? request.TsDate : runit.WryDate;

        }

        runit.AddDomainEvent(new TrackingUnitUpdatedEvent(runit));

        sim.SStatus = SStatus.Installed;
        sim.AddDomainEvent(new SimCardUpdatedEvent(sim));

        sunit.UStatus = UStatus.InstalledInactive;
        sunit.UnitName = asset.TrackedAssetCode;
        sunit.TrackedAssetId = asset.Id;
        sunit.SimCardId = request.SimCardId;
        sunit.InsMode = request.InsMode;

        switch (request.SubPackage)
        {
            case SubPackage.Active:
                {
                  Activate(sunit, serviceLog, request.TsDate, sprice, true);
                    break;
                }
            case SubPackage.ActiveHosting:
                {
                    ActivateForHosting(sunit, serviceLog, request.TsDate, sprice, true);
                    break;
                }
            case SubPackage.ActiveGprs:
                {
                    ActivateForGprs(sunit, serviceLog, request.TsDate, sprice,true);
                    break;
                }
        }

        serviceLog.Amount = serviceLog.Amount + 0.0m;

        serviceLog.AddDomainEvent(new ServiceLogCreatedEvent(serviceLog));

        _context.ServiceLogs.Add(serviceLog);

        sunit.AddDomainEvent(new TrackingUnitUpdatedEvent(sunit));

        var result = await _context.SaveChangesAsync(cancellationToken);
        if (result > 0)
        {
            if (request.ApplyChangesOnWialon)
            {
                //ExcuteRegistredTasks Here
            }
            return await Result<int>.SuccessAsync(sunit.Id);
        }
        else
            return await Result<int>.FailureAsync("TransferTrackingUnit Faild!");



    }
}

