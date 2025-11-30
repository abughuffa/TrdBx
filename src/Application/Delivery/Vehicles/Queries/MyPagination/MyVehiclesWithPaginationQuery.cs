
using CleanArchitecture.Blazor.Application.Features.Vehicles.DTOs;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Caching;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Mappers;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Queries.Pagination;

public class MyVehiclesWithPaginationQuery : VehicleAdvancedFilter, ICacheableRequest<PaginatedData<VehicleDto>>
{
    public override string ToString()
    {
        return $"CurrentUser:{CurrentUser?.UserId}, Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => VehicleCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => VehicleCacheKey.Tags;
    public MyVehicleAdvancedSpecification Specification => new MyVehicleAdvancedSpecification(this);
}
    
public class MyVehiclesWithPaginationQueryHandler :
         IRequestHandler<MyVehiclesWithPaginationQuery, PaginatedData<VehicleDto>>
{
        private readonly IApplicationDbContext _context;

        public MyVehiclesWithPaginationQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedData<VehicleDto>> Handle(MyVehiclesWithPaginationQuery request, CancellationToken cancellationToken)
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