using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Transfer;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Reserve;

public class ReserveTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }

    [Description("CustomerId")] public int CustomerId { get; set; }

    [Description("ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;


    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, ReserveTrackingUnitCommand>(MemberList.None)

    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            //.ForMember(dest => dest.CustomerId, opt => opt.Ignore())
    //            .ForMember(dest => dest.ApplyChangesOnWialon, opt => opt.Ignore());
    //    }
    //}
}

public class ReserveTrackingUnitCommandHandler : IRequestHandler<ReserveTrackingUnitCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ReserveTrackingUnitCommandHandler> _localizer;
    //public ReserveTrackingUnitCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<ReserveTrackingUnitCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ReserveTrackingUnitCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public ReserveTrackingUnitCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<ReserveTrackingUnitCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(ReserveTrackingUnitCommand request, CancellationToken cancellationToken)
    {

        ////await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.New || unit.UStatus == UStatus.Reserved || unit.UStatus == UStatus.Used))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be New/Reserved or used to procced");
        }

        if (unit.UStatus != UStatus.Used) unit.UStatus = UStatus.Reserved;

        unit.CustomerId = request.CustomerId;

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

