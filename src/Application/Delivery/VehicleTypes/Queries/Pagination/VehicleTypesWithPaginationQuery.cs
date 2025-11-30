
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.DTOs;
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Caching;
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Mappers;
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Queries.Pagination;

public class VehicleTypesWithPaginationQuery : VehicleTypeAdvancedFilter, ICacheableRequest<PaginatedData<VehicleTypeDto>>
{
    public override string ToString()
    {
        return $"Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => VehicleTypeCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => VehicleTypeCacheKey.Tags;
    public VehicleTypeAdvancedSpecification Specification => new VehicleTypeAdvancedSpecification(this);
}
    
public class VehicleTypesWithPaginationQueryHandler :
         IRequestHandler<VehicleTypesWithPaginationQuery, PaginatedData<VehicleTypeDto>>
{
        private readonly IApplicationDbContext _context;

        public VehicleTypesWithPaginationQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedData<VehicleTypeDto>> Handle(VehicleTypesWithPaginationQuery request, CancellationToken cancellationToken)
        {
           var data = await _context.VehicleTypes.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                   .ProjectToPaginatedDataAsync(request.Specification, 
                                                                                request.PageNumber, 
                                                                                request.PageSize, 
                                                                                VehicleTypeMapper.ToDto, 
                                                                                cancellationToken);
            return data;
        }
}