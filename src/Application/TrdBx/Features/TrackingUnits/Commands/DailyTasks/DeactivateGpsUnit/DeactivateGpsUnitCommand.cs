using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.DeactivateTrackingUnit;

public class DeactivateTrackingUnitCommand : ICacheInvalidatorRequest<Result>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    //[Description("InstallerId")] public string InstallerId { get; set; } = string.Empty;
    [Description("ApplyChangesToDatabase")] public bool ApplyChangesToDatabase { get; set; } = true;
    [Description("ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
//    private class Mapping : Profile
//    {
//        public Mapping()
//        {
//            CreateMap<TrackingUnitDto, DeactivateTrackingUnitCommand>(MemberList.None)
//                //.ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.TsDate, opt => opt.Ignore())
//                .ForMember(dest => dest.InstallerId, opt => opt.Ignore())
//                .ForMember(dest => dest.ApplyChangesOnWialon, opt => opt.Ignore());

//            CreateMap<DeactivateTestCase, DeactivateTrackingUnitCommand>(MemberList.None)
//.ForMember(x => x.Id, s => s.MapFrom(y => y.TrackingUnitId));
//            //.ForMember(x => x.InstallerId, s => s.Ignore())
//            //.ForMember(x => x.ApplyChangesOnWialon, s => s.Ignore());
//        }
//    }
}
public class DeactivateTrackingUnitCommandHandler : SubscriptionSharedLogic, IRequestHandler<DeactivateTrackingUnitCommand, Result>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<DeactivateTrackingUnitCommandHandler> _localizer;
    //private readonly IWialonService _wialonService;
    //public DeactivateTrackingUnitCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<DeactivateTrackingUnitCommandHandler> localizer,
    //                                     IWialonService wialonService)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _wialonService = wialonService;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<DeactivateTrackingUnitCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public DeactivateTrackingUnitCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<DeactivateTrackingUnitCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result> Handle(DeactivateTrackingUnitCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).Include(x => x.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync(cancellationToken: cancellationToken) ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.InstalledActiveHosting || unit.UStatus == UStatus.InstalledActiveGprs || unit.UStatus == UStatus.InstalledActive))
        {
            return await Result.FailureAsync("Tracking Unit status should be InstalledActive, InstalledActiveGprs Or InstalledActiveHosting to Deactivate it.");
        }

        var price = await  GetCPrice(_context, (int)unit.CustomerId, unit.TrackingUnitModelId);

        var serviceNo = await GenSerialNo(_context, "ServiceLog", request.TsDate);

        //if (request.ApplyChangesToDatabase)
        //{
            var serviceLog = new ServiceLog()
            {
                Desc = string.Format("إلغاء تفعيل الوحدة ({0}).", unit.SNo),
                ServiceNo = serviceNo,
                ServiceTask = ServiceTask.DeactivateUnit,
                CustomerId = (int)unit.CustomerId,
                //InstallerId = request.InstallerId,
                SerDate = request.TsDate,
                Amount = 0.0m,
                IsDeserved = true,
                IsBilled = false,
                Subscriptions = [],
                WialonTasks = []
            };

            var result = Deactivate(unit, serviceLog, request.TsDate, price, request.ApplyChangesToDatabase);

            serviceLog.AddDomainEvent(new ServiceLogCreatedEvent(serviceLog));

        _context.ServiceLogs.Add(serviceLog);

            unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));

            var rowsAffected = await _context.SaveChangesAsync(cancellationToken);

            if (rowsAffected > 0)
            {
                if (request.ApplyChangesOnWialon)
                {
                    //ExcuteRegistredTasks Here
                }
                return await Result.SuccessAsync();
            }
            else
                return await Result.FailureAsync(result);



    }
}

