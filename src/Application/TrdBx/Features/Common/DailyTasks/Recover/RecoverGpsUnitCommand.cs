using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Recover;

public class RecoverTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Display(Name = "CreateDeservedServices")] public bool CreateDeservedServices { get; set; } = false;
    [Display(Name = "ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
}

public class RecoverTrackingUnitCommandHandler : SubscriptionSharedLogic, IRequestHandler<RecoverTrackingUnitCommand, Result<int>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IWialonService _wialonService;

    public RecoverTrackingUnitCommandHandler(
       IApplicationDbContextFactory dbContextFactory,IWialonService wialonService)
    {
       _dbContextFactory = dbContextFactory;
       _wialonService = wialonService;
    }
    public async ValueTask<Result<int>> Handle(RecoverTrackingUnitCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await context.TrackingUnits.Where(x => x.Id == request.Id).Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.InstalledActive || unit.UStatus == UStatus.InstalledActiveGprs || unit.UStatus == UStatus.InstalledActiveHosting || unit.UStatus == UStatus.InstalledInactive))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be Installed to Recover it.");
        }

        var asset = await context.TrackedAssets.Where(x => x.Id == (int)unit.TrackedAssetId).FirstAsync();

        var price = await GetCPrice(context,(int)unit.CustomerId, unit.TrackingUnitModelId);

        var serviceNo = await GenSerialNo(context, "ServiceLog", request.TsDate);

        var serviceLog = new ServiceLog()
        {
            Description = string.Format("استرجاع الوحدة ({0}) من الأصل ({1}).", unit.SNo, asset.TrackedAssetNo),
            ServiceNo = serviceNo,
            ServiceTask = ServiceTask.Recover,
            CustomerId = (int)unit.CustomerId,
            SerDate = request.TsDate,
            IsDeserved = request.CreateDeservedServices,
            IsBilled = false,
            Amount = await GetSPrice(context, ServiceTask.Recover),
            Subscriptions = new List<Subscription>(),
            WialonTasks = new List<WialonTask>()
        };

        if (unit.UStatus == UStatus.InstalledActive || unit.UStatus == UStatus.InstalledActiveHosting || unit.UStatus == UStatus.InstalledActiveGprs)
        {
            Deactivate(unit, serviceLog, request.TsDate, price,true);
        }
        else if (unit.UStatus == UStatus.InstalledInactive && unit.IsOnWialon)
        {
            serviceLog.WialonTasks.Add(new WialonTask()
            {
                TrackingUnitId = unit.Id,
                WialonAPIAction = WialonAPIAction.RemoveFromWialon,
                Description = string.Format("حذف الوحدة ({0}) من منصة ويلون.", unit.SNo),
                ExcDate = request.TsDate,
                IsExecuted = false,
            });
        }

        asset.IsAvaliable = true;

        unit.UnitName = null;
        unit.UStatus = UStatus.Recovered;
        unit.TrackedAssetId = null;
        unit.InsMode = InsMode.Null;

        serviceLog.Amount = 0.0m;

        serviceLog.AddDomainEvent(new ServiceLogCreatedEvent(serviceLog));
        context.ServiceLogs.Add(serviceLog);

        unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));

        var result = await context.SaveChangesAsync(cancellationToken);
        if (result > 0)
        {
            if (request.ApplyChangesOnWialon)
            {
                //ExcuteRegistredTasks Here
            }
            return await Result<int>.SuccessAsync(unit.Id);
        }
        else
            return await Result<int>.FailureAsync("TransferTrackingUnit Faild!");
    }
}

