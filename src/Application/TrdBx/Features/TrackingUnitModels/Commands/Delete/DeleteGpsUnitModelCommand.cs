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

    // private readonly IApplicationDbContext _context;
    // public DeleteTrackingUnitModelCommandHandler(
    //     IApplicationDbContext context
    // )
    // {
    //     _context = context;
    // }
    public async ValueTask<Result> Handle(DeleteTrackingUnitModelCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await context.TrackingUnitModels.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
           // raise a delete domain event
           item.AddDomainEvent(new TrackingUnitModelDeletedEvent(item));
           context.TrackingUnitModels.Remove(item);
        }
        await context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();


    //     var items = await _context.TrackingUnitModels.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
    //     foreach (var item in items)
    //     {
    //         // raise a delete domain event
    //         item.AddDomainEvent(new TrackingUnitModelDeletedEvent(item));
    //         _context.TrackingUnitModels.Remove(item);
    //     }
    //    var result = await _context.SaveChangesAsync(cancellationToken);
    //     return await Result<int>.SuccessAsync(result);
    }



}

