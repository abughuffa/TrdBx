


//using CleanArchitecture.Blazor.Application.Features.Shipments.Caching;
//using CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;

//namespace CleanArchitecture.Blazor.Application.Features.Shipments.Commands.AddEdit;

//public class AddEditShipmentCommand: ICacheInvalidatorRequest<Result<int>>
//{
//      [Description("Id")]
//      public int Id { get; set; }
//    [Description("ShipmentNo")]
//    public string ShipmentNo { get; set; }
//    [Description("StartLocation")]
//    public string StartLocation { get; set; } = $"0.0,0.0";
//    [Description("EndLocation")]
//    public string EndLocation { get; set; } = $"0.0,0.0";
//    [Description("Price")]
//    public decimal Price { get; set; } = 0.0m;
//    [Description("IsBidable")]
//    public bool IsBidable { get; set; } = false;
//    [Description("RecVehicleType")]
//    public int[] RecVehicleType { get; set; } = Array.Empty<int>();


//    public string CacheKey => ShipmentCacheKey.GetAllCacheKey;
//      public IEnumerable<string>? Tags => ShipmentCacheKey.Tags;
//}

//public class AddEditShipmentCommandHandler : IRequestHandler<AddEditShipmentCommand, Result<int>>
//{
//    private readonly IApplicationDbContext _context;
//    public AddEditShipmentCommandHandler(
//        IApplicationDbContext context)
//    {
//        _context = context;
//    }
//    public async Task<Result<int>> Handle(AddEditShipmentCommand request, CancellationToken cancellationToken)
//    {
//        if (request.Id > 0)
//        {
//            var item = await _context.Shipments.FindAsync(request.Id, cancellationToken);
//            if (item == null)
//            {
//                return await Result<int>.FailureAsync($"Shipment with id: [{request.Id}] not found.");
//            }
//            ShipmentMapper.ApplyChangesFrom(request,item);
//			// raise a update domain event
//			item.AddDomainEvent(new ShipmentUpdatedEvent(item));
//            await _context.SaveChangesAsync(cancellationToken);
//            return await Result<int>.SuccessAsync(item.Id);
//        }
//        else
//        {
//            var item = ShipmentMapper.FromEditCommand(request);
//            // raise a create domain event
//			item.AddDomainEvent(new ShipmentCreatedEvent(item));
//            _context.Shipments.Add(item);
//            await _context.SaveChangesAsync(cancellationToken);
//            return await Result<int>.SuccessAsync(item.Id);
//        }
       
//    }
//}

