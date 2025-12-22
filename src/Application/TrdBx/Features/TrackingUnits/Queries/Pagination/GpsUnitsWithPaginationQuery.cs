using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
    using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Queries.Pagination;

public class TrackingUnitsWithPaginationQuery : TrackingUnitAdvancedFilter, ICacheableRequest<PaginatedData<TrackingUnitDto>>
{

    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword},Client/Customer:{CustomerId},UStatus:{UStatus}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }

    public string CacheKey => TrackingUnitCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => TrackingUnitCacheKey.Tags;
    public TrackingUnitAdvancedSpecification Specification => new TrackingUnitAdvancedSpecification(this);

}
    
public class TrackingUnitsWithPaginationQueryHandler :
         IRequestHandler<TrackingUnitsWithPaginationQuery, PaginatedData<TrackingUnitDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public TrackingUnitsWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public TrackingUnitsWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<PaginatedData<TrackingUnitDto>> Handle(TrackingUnitsWithPaginationQuery request, CancellationToken cancellationToken)
        {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.TrackingUnits.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //    .ProjectToPaginatedDataAsync<TrackingUnit, TrackingUnitDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
        //return data;

        var data = await _context.TrackingUnits.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                     .ProjectToPaginatedDataAsync(request.Specification,
                                                                  request.PageNumber,
                                                                  request.PageSize,
                                                                  Mapper.ToDto,
                                                                  cancellationToken);
        return data;
    }
}