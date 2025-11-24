

using CleanArchitecture.Blazor.Application.Features.Shipments.Caching;
using CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Create;

public class CreateShipmentCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("ShipmentNo")]
    public string ShipmentNo { get; set; }
    [Description("StartLocation")]
    public string StartLocation { get; set; } = $"0.0,0.0";
    [Description("EndLocation")]
    public string EndLocation { get; set; } = $"0.0,0.0";
    [Description("Price")]
    public decimal Price { get; set; } = 0.0m;
    [Description("IsBidable")]
    public bool IsBidable { get; set; } = false;
    [Description("RecVehicleType")]
    public int[] RecVehicleType { get; set; } = Array.Empty<int>();

    public string CacheKey => ShipmentCacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => ShipmentCacheKey.Tags;
}
    
    public class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        public CreateShipmentCommandHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<int>> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
        {
           var item = ShipmentMapper.FromCreateCommand(request);
           // raise a create domain event
	       item.AddDomainEvent(new ShipmentCreatedEvent(item));
           _context.Shipments.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  await Result<int>.SuccessAsync(item.Id);
        }
    }

