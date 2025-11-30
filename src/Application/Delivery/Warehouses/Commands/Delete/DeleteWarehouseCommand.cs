

using CleanArchitecture.Blazor.Application.Features.Warehouses.Caching;


namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Commands.Delete;

public class DeleteWarehouseCommand:  ICacheInvalidatorRequest<Result<int>>
{
  public int[] Id {  get; }
  public string CacheKey => WarehouseCacheKey.GetAllCacheKey;
  public IEnumerable<string>? Tags => WarehouseCacheKey.Tags;
  public DeleteWarehouseCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteWarehouseCommandHandler : 
             IRequestHandler<DeleteWarehouseCommand, Result<int>>

{
    private readonly IApplicationDbContext _context;
    public DeleteWarehouseCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Warehouses.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
		    // raise a delete domain event
			item.AddDomainEvent(new WarehouseDeletedEvent(item));
            _context.Warehouses.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

