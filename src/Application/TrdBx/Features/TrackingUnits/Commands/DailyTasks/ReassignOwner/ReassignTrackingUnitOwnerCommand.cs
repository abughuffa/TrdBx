using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ReassignOwner;

public class ReassignTrackingUnitOwnerCommand : ICacheInvalidatorRequest<Result<int>>
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
    //        CreateMap<TrackingUnitDto, ReassignTrackingUnitOwnerCommand>(MemberList.None)

    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            //.ForMember(dest => dest.CustomerId, opt => opt.Ignore())
    //            .ForMember(dest => dest.ApplyChangesOnWialon, opt => opt.Ignore());
    //    }
    //}
}

public class ReassignTrackingUnitOwnerCommandHandler : IRequestHandler<ReassignTrackingUnitOwnerCommand, Result<int>>
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
    private readonly IStringLocalizer<ReassignTrackingUnitOwnerCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public ReassignTrackingUnitOwnerCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<ReassignTrackingUnitOwnerCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(ReassignTrackingUnitOwnerCommand request, CancellationToken cancellationToken)
    {

        ////await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if ((unit.UStatus == UStatus.New || unit.UStatus == UStatus.Reserved))
        {
            return await Result<int>.FailureAsync("Tracking Unit status shouldn't be New or Reserved to procced");
        }

        //if (unit.UStatus != UStatus.Used) unit.UStatus = UStatus.Reserved;

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
            return await Result<int>.FailureAsync("Reassign Owner of TrackingUnit Faild!");


    }
}

