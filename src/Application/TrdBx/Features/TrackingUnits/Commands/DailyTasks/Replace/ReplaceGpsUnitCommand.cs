using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Reserve;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Replace;

public class ReplaceTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Description("SUnitId")] public int SUnitId { get; set; }
    [Description("SimCardId")] public int SimCardId { get; set; } = 0;
    [Description("CustomerId")] public int CustomerId { get; set; }
    [Description("SubPackage")] public SubPackage SubPackage { get; set; } = SubPackage.Active;
    [Description("InsMode")] public InsMode InsMode { get; set; }
    [Description("CreateDeservedServices")] public bool CreateDeservedServices { get; set; }
    [Description("IsTampred")] public bool IsTampred { get; set; }
    [Description("ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    

}

public class ReplaceTrackingUnitCommandHandler : SubscriptionSharedLogic, IRequestHandler<ReplaceTrackingUnitCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ReplaceTrackingUnitCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public ReplaceTrackingUnitCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<ReplaceTrackingUnitCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }



    public async Task<Result<int>> Handle(ReplaceTrackingUnitCommand request, CancellationToken cancellationToken)
    {
        var runit = await _context.TrackingUnits.Where(x => x.Id == request.Id).Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(runit.UStatus == UStatus.InstalledActive || runit.UStatus == UStatus.InstalledActiveHosting || runit.UStatus == UStatus.InstalledActiveGprs || runit.UStatus == UStatus.InstalledInactive))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be Installed to procced");
        }

        var sunit = await _context.TrackingUnits.Where(x => x.Id == request.SUnitId).Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.SUnitId}] not found.");

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

        var asset = await _context.TrackedAssets.FindAsync(new object[] { runit.TrackedAssetId }, cancellationToken) ?? throw new NotFoundException($"TrackedAsset with id: [{runit.TrackedAssetId}] not found.");

        var rprice = await GetCPrice(_context,  (int)runit.CustomerId, runit.TrackingUnitModelId);

        var sprice = await GetCPrice(_context,  (int)sunit.CustomerId, sunit.TrackingUnitModelId);

        List<CPrice> prices = new() { rprice, sprice };

        var T = request.IsTampred;  //IsTampred
        var R = runit.WryDate < request.TsDate; //Replaced Unit Warrenty
        var S = sunit.WryDate is null ? true : sunit.WryDate <= request.TsDate ? true : false; //Selected Unit Warrenty

        var IsObserved = !R || S;
        var Rw = R && S;
        var Sw = !T && R && S;

        var serviceNo = await GenSerialNo(_context, "ServiceLog", request.TsDate);

        var serviceLog = new ServiceLog()
        {
            ServiceNo = serviceNo,
            ServiceTask = ServiceTask.Replace,
            CustomerId = request.CustomerId,
            //InstallerId = request.InstallerId,
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
                    serviceLog.Description = string.Format("استبدال الوحدة ({0}) بالوحدة المستعملة ({1}) للأصل ({2})", runit.SNo, sunit.SNo, asset.TrackedAssetNo);
                    serviceLog.Amount = await GetSPrice(_context, ServiceTask.Replace);

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
                    serviceLog.Description = string.Format("استبدال الوحدة ({0}) بالوحدة الجديدة ({1}) للأصل ({2})", runit.SNo, sunit.SNo, asset.TrackedAssetNo);
                    sunit.WryDate = Sw ? request.TsDate : request.TsDate.AddDays(365);

                    serviceLog.Amount = sprice.Price;
                    break;
                }
        }




       
       
        switch (request.SubPackage)
        {
            case SubPackage.Active:
                {
                    MixSubscriptions(runit,sunit, serviceLog, request.TsDate, prices,384, true);
                    sunit.UStatus = UStatus.InstalledActive;
                    break;
                }
            case SubPackage.ActiveHosting:
                {
                    MixSubscriptions(runit,sunit, serviceLog, request.TsDate, prices,256, true);
                    sunit.UStatus = UStatus.InstalledActiveHosting;
                    break;
                }
            case SubPackage.ActiveGprs:
                {
                    MixSubscriptions(runit,sunit, serviceLog, request.TsDate, prices,128, true);
                    sunit.UStatus = UStatus.InstalledActiveGprs;
                    break;
                }
        }

        if (runit.SimCardId != null && sim.Id == runit.SimCardId)
        {
            runit.SimCardId = null;
        }

        runit.UnitName = null;
        runit.UStatus = UStatus.Recovered;
        runit.TrackedAssetId = null;
        runit.InsMode = InsMode.Null;
        runit.WryDate = Rw ? request.TsDate : runit.WryDate;
        runit.AddDomainEvent(new TrackingUnitUpdatedEvent(runit));

        sim.SStatus = SStatus.Installed;
        sim.AddDomainEvent(new SimCardUpdatedEvent(sim));

        sunit.UnitName = asset.TrackedAssetCode;
        sunit.TrackedAssetId = asset.Id;
        sunit.SimCardId = request.SimCardId;
        sunit.InsMode = request.InsMode;
        sunit.AddDomainEvent(new TrackingUnitUpdatedEvent(sunit));

        serviceLog.Amount = serviceLog.Amount + 0.0m;
        serviceLog.AddDomainEvent(new ServiceLogCreatedEvent(serviceLog));

        _context.ServiceLogs.Add(serviceLog);

        

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

