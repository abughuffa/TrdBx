using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Replace;

public class ReplaceTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Display(Name = "SUnitId")] public int SUnitId { get; set; }
    [Display(Name = "SimCardId")] public int SimCardId { get; set; } = 0;
    [Display(Name = "CustomerId")] public int CustomerId { get; set; }
    [Display(Name = "SubPackage")] public SubPackage SubPackage { get; set; } = SubPackage.Active;
    [Display(Name = "InsMode")] public InsMode InsMode { get; set; }
    [Display(Name = "CreateDeservedServices")] public bool CreateDeservedServices { get; set; }
    [Display(Name = "IsTampred")] public bool IsTampred { get; set; }
    [Display(Name = "ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    

}

public class ReplaceTrackingUnitCommandHandler : SubscriptionSharedLogic, IRequestHandler<ReplaceTrackingUnitCommand, Result<int>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IWialonService _wialonService;

    public ReplaceTrackingUnitCommandHandler(
       IApplicationDbContextFactory dbContextFactory,IWialonService wialonService)
    {
       _dbContextFactory = dbContextFactory;
       _wialonService = wialonService;
    }



    public async ValueTask<Result<int>> Handle(ReplaceTrackingUnitCommand request, CancellationToken cancellationToken)
    {
         await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var runit = await context.TrackingUnits.Where(x => x.Id == request.Id).Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(runit.UStatus == UStatus.InstalledActive || runit.UStatus == UStatus.InstalledActiveHosting || runit.UStatus == UStatus.InstalledActiveGprs || runit.UStatus == UStatus.InstalledInactive))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be Installed to procced");
        }

        var sunit = await context.TrackingUnits.Where(x => x.Id == request.SUnitId).Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.SUnitId}] not found.");

        if (!(sunit.UStatus == UStatus.New || sunit.UStatus == UStatus.Reserved || sunit.UStatus == UStatus.Used))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be New/Reserved or used to procced");
        }

        if (sunit.UStatus == UStatus.New || sunit.UStatus == UStatus.Reserved)
        {
            sunit.Subscriptions = new List<Subscription>();
        }

        sunit.CustomerId = request.CustomerId;

        var sim = await context.SimCards.FindAsync(new object[] { request.SimCardId }, cancellationToken) ?? throw new NotFoundException($"SimCard with id: [{request.SimCardId}] not found.");

        var asset = await context.TrackedAssets.FindAsync(new object[] { runit.TrackedAssetId }, cancellationToken) ?? throw new NotFoundException($"TrackedAsset with id: [{runit.TrackedAssetId}] not found.");

        var rprice = await GetCPrice(context,  (int)runit.CustomerId, runit.TrackingUnitModelId);

        var sprice = await GetCPrice(context,  (int)sunit.CustomerId, sunit.TrackingUnitModelId);

        List<CPrice> prices = new() { rprice, sprice };

        var T = request.IsTampred;  //IsTampred
        var R = runit.WryDate < request.TsDate; //Replaced Unit Warrenty
        var S = sunit.WryDate is null ? true : sunit.WryDate <= request.TsDate ? true : false; //Selected Unit Warrenty

        var IsObserved = !R || S;
        var Rw = R && S;
        var Sw = !T && R && S;

        var serviceNo = await GenSerialNo(context, "ServiceLog", request.TsDate);

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
                    serviceLog.Amount = await GetSPrice(context, ServiceTask.Replace);

                    sunit.WryDate = Sw ? request.TsDate : sunit.WryDate;

                    if (sunit.SimCardId != null && sim.Id != sunit.SimCardId)
                    {
                        var oldSimCard = context.SimCards.Where(a => a.Id == (int)sunit.SimCardId).FirstOrDefault();
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

        context.ServiceLogs.Add(serviceLog);

        

        var result = await context.SaveChangesAsync(cancellationToken);
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

