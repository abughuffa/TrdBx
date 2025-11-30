using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Delete;

public class DeleteTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    public DeleteTrackingUnitCommand(int[] id)
    {
        Id = id;
    }

}

public class DeleteTrackingUnitCommandHandler : IRequestHandler<DeleteTrackingUnitCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    ////private readonly IMapper _mapper;
    //public DeleteTrackingUnitCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    //_mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteTrackingUnitCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteTrackingUnitCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await _context.TrackingUnits.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new TrackingUnitDeletedEvent(item));
        //    _context.TrackingUnits.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();


        var items = await _context.TrackingUnits.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new TrackingUnitDeletedEvent(item));
            _context.TrackingUnits.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

