
using CleanArchitecture.Blazor.Application.Features.Shipments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Shipments.Caching;
using CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;
using CleanArchitecture.Blazor.Application.Features.Shipments.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Queries.Pagination;

public class ShipmentsWithPaginationQuery : ShipmentAdvancedFilter, ICacheableRequest<PaginatedData<ShipmentDto>>
{
    public override string ToString()
    {
        return $"Listview:{ListView}:{CurrentUser?.UserId}-{LocalTimezoneOffset.TotalHours}, Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => ShipmentCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => ShipmentCacheKey.Tags;
    public ShipmentAdvancedSpecification Specification => new ShipmentAdvancedSpecification(this);
}
    
public class ShipmentsWithPaginationQueryHandler :
         IRequestHandler<ShipmentsWithPaginationQuery, PaginatedData<ShipmentDto>>
{
        private readonly IApplicationDbContext _context;

        public ShipmentsWithPaginationQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedData<ShipmentDto>> Handle(ShipmentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
           var data = await _context.Shipments.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                   .ProjectToPaginatedDataAsync(request.Specification, 
                                                                                request.PageNumber, 
                                                                                request.PageSize, 
                                                                                ShipmentMapper.ToDto, 
                                                                                cancellationToken);
            return data;
        }
}