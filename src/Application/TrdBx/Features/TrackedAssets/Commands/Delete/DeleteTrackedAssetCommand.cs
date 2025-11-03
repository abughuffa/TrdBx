using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Delete;

public class DeleteTrackedAssetCommand : ICacheInvalidatorRequest<Result>
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
             IRequestHandler<DeleteTrackedAssetCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteTrackedAssetCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(DeleteTrackedAssetCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await db.TrackedAssets.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new TrackedAssetDeletedEvent(item));
            db.TrackedAssets.Remove(item);
        }
        await db.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();

    }

}

