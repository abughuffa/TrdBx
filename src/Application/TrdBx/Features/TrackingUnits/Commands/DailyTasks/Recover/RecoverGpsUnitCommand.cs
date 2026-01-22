using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Recover;

public class RecoverTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    //[Description("InstallerId")] public string InstallerId { get; set; } = string.Empty;
    [Description("CreateDeservedServices")] public bool CreateDeservedServices { get; set; } = false;
    [Description("ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, RecoverTrackingUnitCommand>(MemberList.None)
    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            .ForMember(dest => dest.TsDate, opt => opt.Ignore())
    //            .ForMember(dest => dest.InstallerId, opt => opt.Ignore())
    //            .ForMember(dest => dest.CreateDeservedServices, opt => opt.Ignore())
    //            .ForMember(dest => dest.ApplyChangesOnWialon, opt => opt.Ignore());
    //    }
    //}

}

public class RecoverTrackingUnitCommandHandler : SubscriptionSharedLogic, IRequestHandler<RecoverTrackingUnitCommand, Result<int>>
{

    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<RecoverTrackingUnitCommandHandler> _localizer;
    //private readonly IWialonService _wialonService;
    //public RecoverTrackingUnitCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<RecoverTrackingUnitCommandHandler> localizer,
    //                                     IWialonService wialonService)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _wialonService = wialonService;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<RecoverTrackingUnitCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public RecoverTrackingUnitCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<RecoverTrackingUnitCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(RecoverTrackingUnitCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.InstalledActive || unit.UStatus == UStatus.InstalledActiveGprs || unit.UStatus == UStatus.InstalledActiveHosting || unit.UStatus == UStatus.InstalledInactive))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be Installed to Recover it.");
        }

        var asset = await _context.TrackedAssets.Where(x => x.Id == (int)unit.TrackedAssetId).FirstAsync();

        var price = await GetCPrice(_context,(int)unit.CustomerId, unit.TrackingUnitModelId);

        var serviceNo = await GenSerialNo(_context, "ServiceLog", request.TsDate);

        var serviceLog = new ServiceLog()
        {
            Desc = string.Format("استرجاع الوحدة ({0}) من الأصل ({1}).", unit.SNo, asset.TrackedAssetNo),
            ServiceNo = serviceNo,
            ServiceTask = ServiceTask.Recover,
            CustomerId = (int)unit.CustomerId,
            //InstallerId = request.InstallerId,
            SerDate = request.TsDate,
            IsDeserved = request.CreateDeservedServices,
            IsBilled = false,
            Amount = await GetSPrice(_context, ServiceTask.Recover),
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
                APITask = APITask.RemoveFromWialon,
                Desc = string.Format("حذف الوحدة ({0}) من منصة ويلون.", unit.SNo),
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
        _context.ServiceLogs.Add(serviceLog);

        unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));

        var result = await _context.SaveChangesAsync(cancellationToken);
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

