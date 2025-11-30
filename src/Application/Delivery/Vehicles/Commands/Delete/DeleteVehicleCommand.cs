

using CleanArchitecture.Blazor.Application.Features.Vehicles.Caching;


namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Commands.Delete;

public class DeleteVehicleCommand:  ICacheInvalidatorRequest<Result<int>>
{
  public int[] Id {  get; }
  public string CacheKey => VehicleCacheKey.GetAllCacheKey;
  public IEnumerable<string>? Tags => VehicleCacheKey.Tags;
  public DeleteVehicleCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteVehicleCommandHandler : 
             IRequestHandler<DeleteVehicleCommand, Result<int>>

{
    private readonly IApplicationDbContext _context;
    public DeleteVehicleCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Vehicles.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
		    // raise a delete domain event
			item.AddDomainEvent(new VehicleDeletedEvent(item));
            _context.Vehicles.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

