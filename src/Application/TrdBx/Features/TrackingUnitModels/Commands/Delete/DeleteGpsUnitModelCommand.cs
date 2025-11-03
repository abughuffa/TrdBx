using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.Delete;

public class DeleteTrackingUnitModelCommand:  ICacheInvalidatorRequest<Result>
{
  public int[] Id {  get; }
   public IEnumerable<string> Tags => TrackingUnitModelCacheKey.Tags;
  public DeleteTrackingUnitModelCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteTrackingUnitModelCommandHandler : 
             IRequestHandler<DeleteTrackingUnitModelCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteTrackingUnitModelCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(DeleteTrackingUnitModelCommand request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await db.TrackingUnitModels.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new TrackingUnitModelDeletedEvent(item));
            db.TrackingUnitModels.Remove(item);
        }
        await db.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

