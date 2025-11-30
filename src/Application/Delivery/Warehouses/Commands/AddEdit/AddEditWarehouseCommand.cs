


using CleanArchitecture.Blazor.Application.Features.Warehouses.Caching;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Commands.AddEdit;

public class AddEditWarehouseCommand: ICacheInvalidatorRequest<Result<int>>
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

public class AddEditWarehouseCommandHandler : IRequestHandler<AddEditWarehouseCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public AddEditWarehouseCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(AddEditWarehouseCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Warehouses.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"Warehouse with id: [{request.Id}] not found.");
            }
            WarehouseMapper.ApplyChangesFrom(request,item);
			// raise a update domain event
			item.AddDomainEvent(new WarehouseUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = WarehouseMapper.FromEditCommand(request);
            // raise a create domain event
			item.AddDomainEvent(new WarehouseCreatedEvent(item));
            _context.Warehouses.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
       
    }
}

