

using CleanArchitecture.Blazor.Application.Features.Shipments.Caching;


namespace CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Delete;

public class DeleteShipmentCommand:  ICacheInvalidatorRequest<Result<int>>
{
  public int[] Id {  get; }
  public string CacheKey => ShipmentCacheKey.GetAllCacheKey;
  public IEnumerable<string>? Tags => ShipmentCacheKey.Tags;
  public DeleteShipmentCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteShipmentCommandHandler : 
             IRequestHandler<DeleteShipmentCommand, Result<int>>

{
    private readonly IApplicationDbContext _context;
    public DeleteShipmentCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteShipmentCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Shipments.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
		    // raise a delete domain event
			item.AddDomainEvent(new ShipmentDeletedEvent(item));
            _context.Shipments.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

