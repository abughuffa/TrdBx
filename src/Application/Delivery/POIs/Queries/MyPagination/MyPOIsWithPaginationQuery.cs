
using CleanArchitecture.Blazor.Application.Features.POIs.DTOs;
using CleanArchitecture.Blazor.Application.Features.POIs.Caching;
using CleanArchitecture.Blazor.Application.Features.POIs.Mappers;
using CleanArchitecture.Blazor.Application.Features.POIs.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.POIs.Queries.Pagination;

public class MyPOIsWithPaginationQuery : POIAdvancedFilter, ICacheableRequest<PaginatedData<POIDto>>
{
    public override string ToString()
    {
        return $"CurrentUser:{CurrentUser?.UserId}, Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => POICacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => POICacheKey.Tags;
    public MyPOIAdvancedSpecification Specification => new MyPOIAdvancedSpecification(this);
}
    
public class MyPOIsWithPaginationQueryHandler :
         IRequestHandler<MyPOIsWithPaginationQuery, PaginatedData<POIDto>>
{
        private readonly IApplicationDbContext _context;

        public MyPOIsWithPaginationQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedData<POIDto>> Handle(MyPOIsWithPaginationQuery request, CancellationToken cancellationToken)
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