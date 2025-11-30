

using CleanArchitecture.Blazor.Application.Features.Vehicles.Caching;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Commands.Create;

public class CreateVehicleCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("VehicleNo")]
    public string VehicleNo { get; set; } = string.Empty;
    [Description("VehicleTypeId")]
    public int VehicleTypeId { get; set; }
    [Description("DriverId")]
    public string DriverId { get; set; }

    public string CacheKey => VehicleCacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => VehicleCacheKey.Tags;
}
    
    public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        public CreateVehicleCommandHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<int>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
           var item = VehicleMapper.FromCreateCommand(request);
           // raise a create domain event
	       item.AddDomainEvent(new VehicleCreatedEvent(item));
           _context.Vehicles.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  await Result<int>.SuccessAsync(item.Id);
        }
    }

