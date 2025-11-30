using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Mappers;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Queries.Export;

public class ExportCusPricesQuery : CusPriceAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
    public CusPriceAdvancedSpecification Specification => new CusPriceAdvancedSpecification(this);
    public IEnumerable<string>? Tags => CusPriceCacheKey.Tags;
    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword}, {OrderBy}, {SortDirection}";
    }
    public string CacheKey => CusPriceCacheKey.GetExportCacheKey($"{this}");

}

public class ExportCusPricesQueryHandler :
         IRequestHandler<ExportCusPricesQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportCusPricesQueryHandler> _localizer;
    //private readonly CusPriceDto _dto = new();
    //public ExportCusPricesQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportCusPricesQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportCusPricesQueryHandler> _localizer;
    private readonly CusPriceDto _dto = new();
    public ExportCusPricesQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportCusPricesQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings

    public async Task<Result<byte[]>> Handle(ExportCusPricesQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.CusPrices.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<CusPriceDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await _context.CusPrices.ApplySpecification(request.Specification)
                       .OrderBy($"{request.OrderBy} {request.SortDirection}")
                       .ProjectTo()
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<CusPriceDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDescription(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDescription(x=>x.CustomerId)],item => item.CustomerId},
                    {_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitModelId)],item => item.TrackingUnitModelId},
                    {_localizer[_dto.GetMemberDescription(x=>x.Gprs)],item => item.Gprs},
                    {_localizer[_dto.GetMemberDescription(x=>x.Host)],item => item.Host},
                    {_localizer[_dto.GetMemberDescription(x=>x.Price)],item => item.Price}

            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);
    }
}
