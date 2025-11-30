using CleanArchitecture.Blazor.Application.Features.Subscriptions.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Commands.Delete;

public class DeleteSubscriptionCommand:  ICacheInvalidatorRequest<Result>
{
  public int[] Id {  get; }
   public IEnumerable<string> Tags => SubscriptionCacheKey.Tags;
  public DeleteSubscriptionCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteSubscriptionCommandHandler : 
             IRequestHandler<DeleteSubscriptionCommand, Result>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    ////private readonly IMapper _mapper;
    //public DeleteSubscriptionCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    //_mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteSubscriptionCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await db.Subscriptions.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new SubscriptionDeletedEvent(item));
        //    db.Subscriptions.Remove(item);
        //}
        //await db.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.Subscriptions.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new SubscriptionDeletedEvent(item));
            _context.Subscriptions.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

