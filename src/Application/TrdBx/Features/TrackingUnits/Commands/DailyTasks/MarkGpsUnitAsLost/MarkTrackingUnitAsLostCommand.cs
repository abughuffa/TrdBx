using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Domain.Enums;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.MarkTrackingUnitAsLost;

public class MarkTrackingUnitAsLostCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Description("CreateDeservedServices")] public bool CreateDeservedServices { get; set; } = false;
    [Description("ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, MarkTrackingUnitAsDamagedCommand>(MemberList.None)
    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            .ForMember(dest => dest.TsDate, opt => opt.Ignore());
    //    }
    //}

}

public class MarkTrackingUnitAsLostCommandHandler :SubscriptionSharedLogic , IRequestHandler<MarkTrackingUnitAsLostCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<MarkTrackingUnitAsDamagedCommandHandler> _localizer;
    //public MarkTrackingUnitAsDamagedCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<MarkTrackingUnitAsDamagedCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<MarkTrackingUnitAsLostCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public MarkTrackingUnitAsLostCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<MarkTrackingUnitAsLostCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(MarkTrackingUnitAsLostCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);


        var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).Include(x => x.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync(cancellationToken) ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");


        //var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (unit.UStatus == UStatus.New || unit.UStatus == UStatus.Reserved || unit.UStatus == UStatus.Lost)
        {
            return await Result<int>.FailureAsync("Tracking Unit status should not be New, Reserved or Lost to procced");
        }

        var asset = await _context.TrackedAssets.Where(x => x.Id == (int)unit.TrackedAssetId).FirstAsync();

        var price = await GetCPrice(_context,(int)unit.CustomerId, unit.TrackingUnitModelId);

        var serviceNo = await GenSerialNo(_context, "ServiceLog", request.TsDate);

        var serviceLog = new ServiceLog()
        {
            Description = string.Empty,
            ServiceNo = serviceNo,
            ServiceTask = ServiceTask.StatusUpdate,
            CustomerId = (int)unit.CustomerId,
            SerDate = request.TsDate,
            IsDeserved = request.CreateDeservedServices,
            IsBilled = false,
            Amount = 0.0m,
            Subscriptions = new List<Subscription>(),
            WialonTasks = new List<WialonTask>()
        };

        if (unit.UStatus == UStatus.InstalledActive || unit.UStatus == UStatus.InstalledActiveHosting || unit.UStatus == UStatus.InstalledActiveGprs)
        {
            serviceLog.Description = string.Format("تعيين كمفقود - بتنفيذ اجراء استرجاع الوحدة ({0}) من الأصل ({1}).", unit.SNo, asset.TrackedAssetNo);
            Deactivate(unit, serviceLog, request.TsDate, price,true);
                    serviceLog.WialonTasks.Add(new WialonTask()
            {
                TrackingUnitId = unit.Id,
                APITask = APITask.CheckOnWialon,
                Description = string.Format(" تحقق من الوحدة ({0}) على منصة ويلون.", unit.SNo),
                ExcDate = request.TsDate,
                IsExecuted = false,
            });


        }
        else if (unit.UStatus == UStatus.InstalledInactive && unit.IsOnWialon)
        {
            serviceLog.Description = string.Format("تعيين كمفقود - بتنفيذ اجراء استرجاع الوحدة ({0}) من الأصل ({1}).", unit.SNo, asset.TrackedAssetNo);
            serviceLog.WialonTasks.Add(new WialonTask()
            {
                TrackingUnitId = unit.Id,
                APITask = APITask.RemoveFromWialon,
                Description = string.Format("حذف الوحدة ({0}) من منصة ويلون.", unit.SNo),
                ExcDate = request.TsDate,
                IsExecuted = false,
            });
                    serviceLog.WialonTasks.Add(new WialonTask()
            {
                TrackingUnitId = unit.Id,
                APITask = APITask.CheckOnWialon,
                Description = string.Format(" تحقق من الوحدة ({0}) على منصة ويلون.", unit.SNo),
                ExcDate = request.TsDate,
                IsExecuted = false,
            });
        }



        asset.IsAvaliable = true;

        unit.UnitName = null;
        unit.UStatus = UStatus.Lost;
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
            return await Result<int>.FailureAsync("MarkTrackingUnitAsLost Faild!");




    }
}

