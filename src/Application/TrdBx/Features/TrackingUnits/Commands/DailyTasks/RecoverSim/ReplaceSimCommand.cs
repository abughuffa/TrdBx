using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Domain.Enums;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.RecoverSim;

public class RecoverSimCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Description("ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;


    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, ReassignTrackingUnitOwnerCommand>(MemberList.None)

    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            //.ForMember(dest => dest.CustomerId, opt => opt.Ignore())
    //            .ForMember(dest => dest.ApplyChangesOnWialon, opt => opt.Ignore());
    //    }
    //}
}

public class RecoverSimCommandHandler :PriceSharedLogic, IRequestHandler<RecoverSimCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ReassignTrackingUnitOwnerCommandHandler> _localizer;
    //public ReassignTrackingUnitOwnerCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<ReassignTrackingUnitOwnerCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<RecoverSimCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public RecoverSimCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<RecoverSimCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(RecoverSimCommand request, CancellationToken cancellationToken)
    {

        ////await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (unit.UStatus == UStatus.InstalledActive || unit.UStatus == UStatus.InstalledActiveGprs || unit.UStatus == UStatus.InstalledActiveHosting)
        {
            return await Result<int>.FailureAsync("Tracking Unit status shouldn't Installed active to procced");
        }

        if ((unit.SimCardId is null))
        {
            return await Result<int>.FailureAsync("This Tracking Unit has not a Sim card installed you can recover it");
        }

        var sim = await _context.SimCards.Where(x => x.Id == unit.SimCardId).FirstAsync() ?? throw new NotFoundException($"SimCard with id: [{unit.SimCardId}] not found.");

        sim.SStatus = SStatus.Recovered;

        sim.AddDomainEvent(new SimCardUpdatedEvent(sim));

        //*****************************
        var serviceNo = await GenSerialNo(_context, "ServiceLog", request.TsDate);

        var serviceLog = new ServiceLog()
        {
            Description = string.Format("استرجاع شريحة الاتصال ({0}) من الوحدة ({1}).", sim.SimCardNo, unit.SNo),
            ServiceNo = serviceNo,
            ServiceTask = ServiceTask.RecoverSimCard,
            CustomerId = (int)unit.CustomerId,
            //InstallerId = request.InstallerId,
            SerDate = request.TsDate,
            IsDeserved = false,
            IsBilled = false,
            Amount = 0.0m,
            Subscriptions = new List<Subscription>(),
            WialonTasks = new List<WialonTask>()
        };

        if (unit.IsOnWialon)
        serviceLog.WialonTasks.Add(new WialonTask()
                    {
                        TrackingUnitId = unit.Id,
                        APITask = APITask.UpdateOnWialon,
                        Description = string.Format("حدث بيانات الوحدة ({0}) على منصة ويلون.", unit.SNo),
                        ExcDate = request.TsDate,
                        IsExecuted = false,
                    });
        //*****************************

        serviceLog.AddDomainEvent(new ServiceLogCreatedEvent(serviceLog));

        _context.ServiceLogs.Add(serviceLog);
        
        unit.SimCardId = null;
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
            return await Result<int>.FailureAsync("Replacing Sim Card for TrackingUnit Faild!");


    }
}

