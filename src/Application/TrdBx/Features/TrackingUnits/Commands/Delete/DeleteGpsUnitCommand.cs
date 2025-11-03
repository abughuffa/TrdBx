using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Delete;

public class DeleteTrackingUnitCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    public DeleteTrackingUnitCommand(int[] id)
    {
        Id = id;
    }

}

public class DeleteTrackingUnitCommandHandler : IRequestHandler<DeleteTrackingUnitCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    public DeleteTrackingUnitCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
        //_mapper = mapper;
    }
    public async Task<Result> Handle(DeleteTrackingUnitCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await db.TrackingUnits.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new TrackingUnitDeletedEvent(item));
            db.TrackingUnits.Remove(item);
        }
        await db.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

