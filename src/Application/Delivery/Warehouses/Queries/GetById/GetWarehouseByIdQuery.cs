
using CleanArchitecture.Blazor.Application.Features.Warehouses.DTOs;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Caching;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Mappers;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Queries.GetById;

public class GetWarehouseByIdQuery : ICacheableRequest<Result<WarehouseDto>>
{
   public required int Id { get; set; }
   public string CacheKey => WarehouseCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string>? Tags => WarehouseCacheKey.Tags;
}

public class GetWarehouseByIdQueryHandler :
     IRequestHandler<GetWarehouseByIdQuery, Result<WarehouseDto>>
{
    private readonly IApplicationDbContext _context;

    public GetWarehouseByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<WarehouseDto>> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Warehouses.ApplySpecification(new WarehouseByIdSpecification(request.Id))
                                                .ProjectTo()
                                                .FirstAsync(cancellationToken);
        return await Result<WarehouseDto>.SuccessAsync(data);
    }
}
