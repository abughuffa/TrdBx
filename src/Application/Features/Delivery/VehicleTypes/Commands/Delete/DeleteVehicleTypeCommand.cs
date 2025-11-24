

using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Caching;


namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Commands.Delete;

public class DeleteVehicleTypeCommand:  ICacheInvalidatorRequest<Result<int>>
{
  public int[] Id {  get; }
  public string CacheKey => VehicleTypeCacheKey.GetAllCacheKey;
  public IEnumerable<string>? Tags => VehicleTypeCacheKey.Tags;
  public DeleteVehicleTypeCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteVehicleTypeCommandHandler : 
             IRequestHandler<DeleteVehicleTypeCommand, Result<int>>

{
    private readonly IApplicationDbContext _context;
    public DeleteVehicleTypeCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.VehicleTypes.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
		    // raise a delete domain event
			item.AddDomainEvent(new VehicleTypeDeletedEvent(item));
            _context.VehicleTypes.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

