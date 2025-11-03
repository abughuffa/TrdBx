using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;


namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Delete;

public class DeleteCusPriceCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }
    public string CacheKey => CusPriceCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => CusPriceCacheKey.Tags;
    public DeleteCusPriceCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteCusPriceCommandHandler :
             IRequestHandler<DeleteCusPriceCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteCusPriceCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(DeleteCusPriceCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await db.CusPrices.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new CusPriceDeletedEvent(item));
            db.CusPrices.Remove(item);
        }
        await db.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();

    }

}

