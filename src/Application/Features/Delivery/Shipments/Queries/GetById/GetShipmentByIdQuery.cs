
using CleanArchitecture.Blazor.Application.Features.Shipments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Shipments.Caching;
using CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;
using CleanArchitecture.Blazor.Application.Features.Shipments.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Queries.GetById;

public class GetShipmentByIdQuery : ICacheableRequest<Result<ShipmentDto>>
{
   public required int Id { get; set; }
   public string CacheKey => ShipmentCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string>? Tags => ShipmentCacheKey.Tags;
}

public class GetShipmentByIdQueryHandler :
     IRequestHandler<GetShipmentByIdQuery, Result<ShipmentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetShipmentByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ShipmentDto>> Handle(GetShipmentByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Shipments.ApplySpecification(new ShipmentByIdSpecification(request.Id))
                                                .ProjectTo()
                                                .FirstAsync(cancellationToken);
        return await Result<ShipmentDto>.SuccessAsync(data);
    }
}
