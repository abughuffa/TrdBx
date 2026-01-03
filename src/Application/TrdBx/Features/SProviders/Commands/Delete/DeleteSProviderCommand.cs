using CleanArchitecture.Blazor.Application.Features.SProviders.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.SProviders.Commands.Delete;

public class DeleteSProviderCommand:  ICacheInvalidatorRequest<Result<int>>
{
  public int[] Id {  get; }
  public IEnumerable<string> Tags => SProviderCacheKey.Tags;
  public DeleteSProviderCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteSProviderCommandHandler : 
             IRequestHandler<DeleteSProviderCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteSProviderCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteSProviderCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteSProviderCommand request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await db.SProviders.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new SProviderDeletedEvent(item));
        //    db.SProviders.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.SProviders.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new SProviderDeletedEvent(item));
            _context.SProviders.Remove(item);
        }
       int result =  await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

