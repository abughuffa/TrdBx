using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;


namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Delete;

public class DeleteTrackedAssetCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }
    public string CacheKey => TrackedAssetCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => TrackedAssetCacheKey.Tags;
    public DeleteTrackedAssetCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteTrackedAssetCommandHandler :
             IRequestHandler<DeleteTrackedAssetCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteTrackedAssetCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteTrackedAssetCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteTrackedAssetCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await _context.TrackedAssets.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new TrackedAssetDeletedEvent(item));
        //    _context.TrackedAssets.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.TrackedAssets.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new TrackedAssetDeletedEvent(item));
            _context.TrackedAssets.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);

    }

}

