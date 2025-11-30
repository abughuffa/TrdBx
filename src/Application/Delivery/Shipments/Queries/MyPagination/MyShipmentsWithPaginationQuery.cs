
using CleanArchitecture.Blazor.Application.Features.Shipments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Shipments.Caching;
using CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;
using CleanArchitecture.Blazor.Application.Features.Shipments.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Queries.Pagination;

public class MyShipmentsWithPaginationQuery : ShipmentAdvancedFilter, ICacheableRequest<PaginatedData<ShipmentDto>>
{
    public override string ToString()
    {
        return $"Listview:{ListView}:{CurrentUser?.UserId}-{LocalTimezoneOffset.TotalHours}, Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => ShipmentCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => ShipmentCacheKey.Tags;
    public MyShipmentAdvancedSpecification Specification => new MyShipmentAdvancedSpecification(this);
}
    
public class MyShipmentsWithPaginationQueryHandler :
         IRequestHandler<MyShipmentsWithPaginationQuery, PaginatedData<ShipmentDto>>
{
        private readonly IApplicationDbContext _context;

        public MyShipmentsWithPaginationQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedData<ShipmentDto>> Handle(MyShipmentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
        var data = await _context.Shipments.Include(s => s.VehicleTypes)
               .ThenInclude(svt => svt.VehicleType).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                .ProjectToPaginatedDataAsync(request.Specification,
                                                                             request.PageNumber,
                                                                             request.PageSize,
                                                                             ShipmentMapper.ToDto,
                                                                             cancellationToken);
        return data;

        //var query = _context.Shipments
        //    .Include(s => s.VehicleTypes)
        //        .ThenInclude(svt => svt.VehicleType)
        //    .Specify(new MyShipmentsSpecification(request))
        //    .OrderBy($"{request.OrderBy} {request.SortDirection}");

        //var totalItems = await query.CountAsync(cancellationToken);

        //var items = await query
        //    .Skip((request.PageNumber - 1) * request.PageSize)
        //    .Take(request.PageSize)
        //    .ToListAsync(cancellationToken);

        //var dtos = items.ToDtosWithVehicleTypes();
        //return new PaginatedData<ShipmentDto>(dtos, totalItems, request.PageNumber, request.PageSize);
    }
}