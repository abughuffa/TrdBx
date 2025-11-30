
using CleanArchitecture.Blazor.Application.Features.Vehicles.DTOs;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Caching;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Mappers;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Queries.Pagination;

public class VehiclesWithPaginationQuery : VehicleAdvancedFilter, ICacheableRequest<PaginatedData<VehicleDto>>
{
    public override string ToString()
    {
        return $"CurrentUser:{CurrentUser?.UserId}, Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => VehicleCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => VehicleCacheKey.Tags;
    public VehicleAdvancedSpecification Specification => new VehicleAdvancedSpecification(this);
}
    
public class VehiclesWithPaginationQueryHandler :
         IRequestHandler<VehiclesWithPaginationQuery, PaginatedData<VehicleDto>>
{
        private readonly IApplicationDbContext _context;

        public VehiclesWithPaginationQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedData<VehicleDto>> Handle(VehiclesWithPaginationQuery request, CancellationToken cancellationToken)
        {
           var data = await _context.Vehicles.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                   .ProjectToPaginatedDataAsync(request.Specification, 
                                                                                request.PageNumber, 
                                                                                request.PageSize, 
                                                                                VehicleMapper.ToDto, 
                                                                                cancellationToken);
            return data;
        }
}