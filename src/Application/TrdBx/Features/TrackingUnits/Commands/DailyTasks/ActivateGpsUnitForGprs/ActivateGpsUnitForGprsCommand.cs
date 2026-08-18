using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnitForGprs;

public class ActivateTrackingUnitForGprsCommand :  ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    //[Display(Name = "InstallerId")] public string InstallerId { get; set; } = string.Empty;
    [Display(Name = "ApplyChangesToDatabase")] public bool ApplyChangesToDatabase { get; set; } = true;
    [Display(Name = "ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, ActivateTrackingUnitForGprsCommand>(MemberList.None)
    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            .ForMember(dest => dest.TsDate, opt => opt.Ignore())
    //            .ForMember(dest => dest.InstallerId, opt => opt.Ignore())
    //            .ForMember(dest => dest.ApplyChangesOnWialon, opt => opt.Ignore());

    //        CreateMap<ActivateGprsTestCase, ActivateTrackingUnitForGprsCommand>(MemberList.None)
    //     .ForMember(x => x.Id, s => s.MapFrom(y => y.TrackingUnitId));
    //        //.ForMember(x => x.InstallerId, s => s.Ignore())
    //        //.ForMember(x => x.ApplyChangesOnWialon, s => s.Ignore());
    //    }
    //}
}

public class ActivateTrackingUnitForGprsCommandHandler : SubscriptionSharedLogic, IRequestHandler<ActivateTrackingUnitForGprsCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ActivateTrackingUnitForGprsCommandHandler> _localizer;
    //private readonly IWialonService _wialonService;
    //public ActivateTrackingUnitForGprsCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<ActivateTrackingUnitForGprsCommandHandler> localizer,
    //                                     IWialonService wialonService)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _wialonService = wialonService;
    //}

    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IWialonService _wialonService;

    public ActivateTrackingUnitForGprsCommandHandler(
       IApplicationDbContextFactory dbContextFactory,IWialonService wialonService)
    {
       _dbContextFactory = dbContextFactory;
       _wialonService = wialonService;
    }
    public async ValueTask<Result<int>> Handle(ActivateTrackingUnitForGprsCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await context.TrackingUnits.Where(x => x.Id == request.Id).Include(x => x.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync(cancellationToken) ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.InstalledActive || unit.UStatus == UStatus.InstalledActiveHosting || unit.UStatus == UStatus.InstalledInactive))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be InstalledInactive, InstalledActive Or InstalledActiveHosting to Activate it for Gprs.");
        }

        var price = await GetCPrice(context, (int)unit.CustomerId, unit.TrackingUnitModelId);

        var serviceNo = await GenSerialNo(context, "ServiceLog", request.TsDate);

        var serviceLog = new ServiceLog()
        {
            Description = string.Format("تفعيل الوحدة ({0}) للتمديد .", unit.SNo),
            ServiceNo = serviceNo,
            ServiceTask = ServiceTask.ActivateUnitForGprs,
            CustomerId = (int)unit.CustomerId,
            //InstallerId = request.InstallerId,
            SerDate = request.TsDate,
            Amount = 0.0m,
            IsDeserved = true,
            IsBilled = false,
            Subscriptions = [],
            WialonTasks = []
        };

        ActivateForGprs(unit, serviceLog, request.TsDate, price, request.ApplyChangesToDatabase);

        if (serviceLog.Subscriptions.Count == 0) serviceLog.IsDeserved = false;

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

