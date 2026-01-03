using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Specifications;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Mappers;
namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Queries.Pagination;






public class WialonUnitsWithPaginationQuery : WialonUnitAdvancedFilter, ICacheableRequest<PaginatedData<WialonUnitDto>>
{

    public IEnumerable<string>? Tags => WialonUnitCacheKey.Tags;
    public WialonUnitAdvancedSpecification Specification => new(this);
    public string CacheKey => WialonUnitCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }

}

public class WialonUnitsWithPaginationQueryHandler :
         IRequestHandler<WialonUnitsWithPaginationQuery, PaginatedData<WialonUnitDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public WialonUnitsWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public WialonUnitsWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<PaginatedData<WialonUnitDto>> Handle(WialonUnitsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //PaginatedData<WialonUnitDto> data;
        switch (request.ListView)
        {
            case WialonUnitListView.All:
                {
                    //var data = await _context.WialonUnits.OrderBy($"{request.OrderBy} {request.SortDirection}")
                    //                    .ProjectToPaginatedDataAsync<WialonUnit, WialonUnitDto>(request.Specification,
                    //                      request.PageNumber,
                    //                      request.PageSize,
                    //                      _mapper.ConfigurationProvider,
                    //                      cancellationToken);
                    var data = await _context.WialonUnits.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                               .ProjectToPaginatedDataAsync(request.Specification,
                                                                            request.PageNumber,
                                                                            request.PageSize,
                                                                            Mapper.ToDto,
                                                                            cancellationToken);
          
                    return data;
                }
            case WialonUnitListView.UnitsNotExistOnTrdBx:
                {
                    var tUnitSNo = await _context.TrackingUnits.Select(o => o.SNo).ToListAsync(cancellationToken);

                    //var data = await _context.WialonUnits.Where(o => !tUnitSNo.Contains(o.UnitSNo))
                    //                            .OrderBy($"{request.OrderBy} {request.SortDirection}")
                    //                         .ProjectToPaginatedDataAsync<WialonUnit, WialonUnitDto>(request.Specification,
                    //                                request.PageNumber,
                    //                                request.PageSize,
                    //                                _mapper.ConfigurationProvider,
                    //                                cancellationToken);

                    var data = await _context.WialonUnits.Where(o => !tUnitSNo.Contains(o.UnitSNo))
                        .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                              .ProjectToPaginatedDataAsync(request.Specification,
                                                                           request.PageNumber,
                                                                           request.PageSize,
                                                                           Mapper.ToDto,
                                                                           cancellationToken);

                    return data;
                }
            case WialonUnitListView.UnitsWithSimCardNotExistOnLibyana:
                {
                    var lSims = await _context.LibyanaSimCards.Select(o => o.SimCardNo).ToListAsync(cancellationToken);

                    //var data = await _context.WialonUnits.Where(o => !lSims.Contains(o.SimCardNo))
                    //                           .OrderBy($"{request.OrderBy} {request.SortDirection}")
                    //                      .ProjectToPaginatedDataAsync<WialonUnit, WialonUnitDto>(request.Specification,
                    //                                request.PageNumber,
                    //                                request.PageSize,
                    //                                _mapper.ConfigurationProvider,
                    //                                cancellationToken);
                    var data = await _context.WialonUnits.Where(o => !lSims.Contains(o.SimCardNo))
                    .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                          .ProjectToPaginatedDataAsync(request.Specification,
                                                                       request.PageNumber,
                                                                       request.PageSize,
                                                                       Mapper.ToDto,
                                                                       cancellationToken);
                    return data;
                }
            default:
                {
                    return null;
                }
        }

    }
}