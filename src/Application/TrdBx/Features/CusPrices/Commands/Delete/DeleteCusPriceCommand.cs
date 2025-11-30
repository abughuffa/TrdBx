using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Delete;

public class DeleteCusPriceCommand : ICacheInvalidatorRequest<Result<int>>
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
             IRequestHandler<DeleteCusPriceCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteCusPriceCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteCusPriceCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteCusPriceCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await _context.CusPrices.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new CusPriceDeletedEvent(item));
            _context.CusPrices.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);

    }

}

