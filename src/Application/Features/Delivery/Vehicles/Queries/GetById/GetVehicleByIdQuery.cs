
using CleanArchitecture.Blazor.Application.Features.Vehicles.DTOs;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Caching;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Mappers;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Queries.GetById;

public class GetVehicleByIdQuery : ICacheableRequest<Result<VehicleDto>>
{
   public required int Id { get; set; }
   public string CacheKey => VehicleCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string>? Tags => VehicleCacheKey.Tags;
}

public class GetVehicleByIdQueryHandler :
     IRequestHandler<GetVehicleByIdQuery, Result<VehicleDto>>
{
    private readonly IApplicationDbContext _context;

    public GetVehicleByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<VehicleDto>> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Vehicles.ApplySpecification(new VehicleByIdSpecification(request.Id))
                                                .ProjectTo()
                                                .FirstAsync(cancellationToken);
        return await Result<VehicleDto>.SuccessAsync(data);
    }
}
