
using CleanArchitecture.Blazor.Application.Features.Warehouses.DTOs;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Caching;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Mappers;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Queries.Pagination;

public class MyWarehousesWithPaginationQuery : WarehouseAdvancedFilter, ICacheableRequest<PaginatedData<WarehouseDto>>
{
    public override string ToString()
    {
        return $"CurrentUser:{CurrentUser?.UserId}, Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => WarehouseCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => WarehouseCacheKey.Tags;
    public MyWarehouseAdvancedSpecification Specification => new MyWarehouseAdvancedSpecification(this);
}
    
public class MyWarehousesWithPaginationQueryHandler :
         IRequestHandler<MyWarehousesWithPaginationQuery, PaginatedData<WarehouseDto>>
{
        private readonly IApplicationDbContext _context;

        public MyWarehousesWithPaginationQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedData<WarehouseDto>> Handle(MyWarehousesWithPaginationQuery request, CancellationToken cancellationToken)
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