

using CleanArchitecture.Blazor.Application.Features.Warehouses.Caching;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Commands.Create;

public class CreateWarehouseCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = string.Empty;
    [Description("Latitude")]
    public double Latitude { get; set; } = 32.8877;
    [Description("Longitude")]
    public double Longitude { get; set; } = 13.1872;




    public string CacheKey => WarehouseCacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => WarehouseCacheKey.Tags;
}
    
    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        public CreateWarehouseCommandHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<int>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
           var item = WarehouseMapper.FromCreateCommand(request);
           // raise a create domain event
	       item.AddDomainEvent(new WarehouseCreatedEvent(item));
           _context.Warehouses.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  await Result<int>.SuccessAsync(item.Id);
        }
    }

