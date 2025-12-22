
using CleanArchitecture.Blazor.Application.Features.POIs.DTOs;
using CleanArchitecture.Blazor.Application.Features.POIs.Caching;
using CleanArchitecture.Blazor.Application.Features.POIs.Mappers;
using CleanArchitecture.Blazor.Application.Features.POIs.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.POIs.Queries.Pagination;

public class POIsWithPaginationQuery : POIAdvancedFilter, ICacheableRequest<PaginatedData<POIDto>>
{
    public override string ToString()
    {
        return $"CurrentUser:{CurrentUser?.UserId}Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => POICacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => POICacheKey.Tags;
    public POIAdvancedSpecification Specification => new POIAdvancedSpecification(this);
}
    
public class POIsWithPaginationQueryHandler :
         IRequestHandler<POIsWithPaginationQuery, PaginatedData<POIDto>>
{
        private readonly IApplicationDbContext _context;

        public POIsWithPaginationQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedData<POIDto>> Handle(POIsWithPaginationQuery request, CancellationToken cancellationToken)
        {
           var data = await _context.POIs.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                   .ProjectToPaginatedDataAsync(request.Specification, 
                                                                                request.PageNumber, 
                                                                                request.PageSize, 
                                                                                POIMapper.ToDto, 
                                                                                cancellationToken);
            return data;
        }
}