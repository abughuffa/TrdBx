using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Update;

public class UpdateTrackingUnitCommand: ICacheInvalidatorRequest<Result<int>>
{
      [Description("Id")]
      public int Id { get; set; }
    [Description("SNo")]
    public required string SNo { get; set; }
    [Description("Imei")]
    public required string Imei { get; set; }
    [Description("TrackingUnitModelId")]
    public int TrackingUnitModelId { get; set; }


    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
      public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateTrackingUnitCommand, TrackingUnit>(MemberList.None);
            CreateMap<TrackingUnitDto, UpdateTrackingUnitCommand>(MemberList.None);
        }
    }
}

public class UpdateTrackingUnitCommandHandler : IRequestHandler<UpdateTrackingUnitCommand, Result<int>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public UpdateTrackingUnitCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(UpdateTrackingUnitCommand request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await db.TrackingUnits.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("TrackingUnit not found");
        _mapper.Map(request, item);
        // raise a update domain event
        item.AddDomainEvent(new TrackingUnitUpdatedEvent(item));
        await db.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}

