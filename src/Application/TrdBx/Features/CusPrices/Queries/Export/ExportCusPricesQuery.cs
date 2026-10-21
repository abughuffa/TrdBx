using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
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
    private readonly TypeAdapterConfig _typeAdapterConfig;
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportCusPricesQueryHandler> _localizer;
    private readonly CusPriceDto _dto = new();
        public ExportCusPricesQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory,
            IExcelService excelService,
            IStringLocalizer<ExportCusPricesQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings

    public async ValueTask<Result<byte[]>> Handle(ExportCusPricesQuery request, CancellationToken cancellationToken)
    {
         await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await context.CusPrices.ApplySpecification(request.Specification)
                       .OrderBy($"{request.OrderBy} {request.SortDirection}")
                       .ProjectToType<CusPriceDto>(_typeAdapterConfig)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<CusPriceDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.CustomerId)],item => item.CustomerId},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.TrackingUnitModelId)],item => item.TrackingUnitModelId},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Gprs)],item => item.Gprs},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Host)],item => item.Host},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Price)],item => item.Price}

            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);
    }
}
