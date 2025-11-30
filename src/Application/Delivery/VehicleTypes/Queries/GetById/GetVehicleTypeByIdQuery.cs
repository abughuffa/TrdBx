
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.DTOs;
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Caching;
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Mappers;
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Queries.GetById;

public class GetVehicleTypeByIdQuery : ICacheableRequest<Result<VehicleTypeDto>>
{
   public required int Id { get; set; }
   public string CacheKey => VehicleTypeCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string>? Tags => VehicleTypeCacheKey.Tags;
}

public class GetVehicleTypeByIdQueryHandler :
     IRequestHandler<GetVehicleTypeByIdQuery, Result<VehicleTypeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetVehicleTypeByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<VehicleTypeDto>> Handle(GetVehicleTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.VehicleTypes.ApplySpecification(new VehicleTypeByIdSpecification(request.Id))
                                                .ProjectTo()
                                                .FirstAsync(cancellationToken);
        return await Result<VehicleTypeDto>.SuccessAsync(data);
    }
}
