
using CleanArchitecture.Blazor.Application.Features.Warehouses.DTOs;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Caching;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Mappers;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Queries.Pagination;

public class WarehousesWithPaginationQuery : WarehouseAdvancedFilter, ICacheableRequest<PaginatedData<WarehouseDto>>
{
    public override string ToString()
    {
        return $"CurrentUser:{CurrentUser?.UserId}Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => WarehouseCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => WarehouseCacheKey.Tags;
    public WarehouseAdvancedSpecification Specification => new WarehouseAdvancedSpecification(this);
}
    
public class WarehousesWithPaginationQueryHandler :
         IRequestHandler<WarehousesWithPaginationQuery, PaginatedData<WarehouseDto>>
{
        private readonly IApplicationDbContext _context;

        public WarehousesWithPaginationQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedData<WarehouseDto>> Handle(WarehousesWithPaginationQuery request, CancellationToken cancellationToken)
        {
           var data = await _context.Warehouses.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                   .ProjectToPaginatedDataAsync(request.Specification, 
                                                                                request.PageNumber, 
                                                                                request.PageSize, 
                                                                                WarehouseMapper.ToDto, 
                                                                                cancellationToken);
            return data;
        }
}