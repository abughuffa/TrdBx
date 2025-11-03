using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Create;

public class CreateTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("SNo")]
    public  string SNo { get; set; } = string.Empty;
    [Description("Imei")]
    public  string Imei { get; set; } = string.Empty;
    [Description("TrackingUnitModelId")]
    public int TrackingUnitModelId { get; set; }

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateTrackingUnitCommand, TrackingUnit>(MemberList.None);
        }
    }
}

public class CreateTrackingUnitCommandHandler : IRequestHandler<CreateTrackingUnitCommand, Result<int>>
{

    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public CreateTrackingUnitCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }


    public async Task<Result<int>> Handle(CreateTrackingUnitCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = _mapper.Map<TrackingUnit>(request);
        // raise a create domain event
        item.AddDomainEvent(new TrackingUnitCreatedEvent(item));
        db.TrackingUnits.Add(item);
        await db.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}

