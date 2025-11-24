

using CleanArchitecture.Blazor.Application.Features.Vehicles.Caching;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Commands.Update;

public class UpdateVehicleCommand: ICacheInvalidatorRequest<Result<int>>
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

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public UpdateVehicleCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {

       var item = await _context.Vehicles.FindAsync(request.Id, cancellationToken);
       if (item == null)
       {
           return await Result<int>.FailureAsync($"Vehicle with id: [{request.Id}] not found.");
       }
       VehicleMapper.ApplyChangesFrom(request, item);
	    // raise a update domain event
	   item.AddDomainEvent(new VehicleUpdatedEvent(item));
       await _context.SaveChangesAsync(cancellationToken);
       return await Result<int>.SuccessAsync(item.Id);
    }
}

