

using CleanArchitecture.Blazor.Application.Features.Warehouses.Caching;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Commands.Update;

public class UpdateWarehouseCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = string.Empty;
    [Description("Latitude")]
    public double Latitude { get; set; } = 0.0;
    [Description("Longitude")]
    public double Longitude { get; set; } = 0.0;

    public string CacheKey => WarehouseCacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => WarehouseCacheKey.Tags;

}

public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public UpdateWarehouseCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {

       var item = await _context.Warehouses.FindAsync(request.Id, cancellationToken);
       if (item == null)
       {
           return await Result<int>.FailureAsync($"Warehouse with id: [{request.Id}] not found.");
       }
       WarehouseMapper.ApplyChangesFrom(request, item);
	    // raise a update domain event
	   item.AddDomainEvent(new WarehouseUpdatedEvent(item));
       await _context.SaveChangesAsync(cancellationToken);
       return await Result<int>.SuccessAsync(item.Id);
    }
}

