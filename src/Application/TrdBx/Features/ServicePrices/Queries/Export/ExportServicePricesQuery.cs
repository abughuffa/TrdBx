using CleanArchitecture.Blazor.Application.Features.ServicePrices.Caching;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;
// using CleanArchitecture.Blazor.Application.Features.ServicePrices.Mappers;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Queries.Export;

public class ExportServicePricesQuery : ServicePriceAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
      public ServicePriceAdvancedSpecification Specification => new ServicePriceAdvancedSpecification();
      public IEnumerable<string> Tags => ServicePriceCacheKey.Tags;
    public override string ToString()
    {
        return $"Search:{Keyword}, {OrderBy}, {SortDirection}";
    }
    public string CacheKey => ServicePriceCacheKey.GetExportCacheKey($"{this}");
}
    
public class ExportServicePricesQueryHandler :
         IRequestHandler<ExportServicePricesQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportServicePricesQueryHandler> _localizer;
    //private readonly ServicePriceDto _dto = new();
    //public ExportServicePricesQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportServicePricesQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly TypeAdapterConfig _typeAdapterConfig;
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportServicePricesQueryHandler> _localizer;
    private readonly ServicePriceDto _dto = new();
        public ExportServicePricesQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory,
            IExcelService excelService,
            IStringLocalizer<ExportServicePricesQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(ExportServicePricesQuery request, CancellationToken cancellationToken)
        {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.ServicePrices.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<ServicePriceDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await context.ServicePrices.ApplySpecification(request.Specification)
    .OrderBy($"{request.OrderBy} {request.SortDirection}")
     .ProjectToType<ServicePriceDto>(_typeAdapterConfig)
    .AsNoTracking()
    .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<ServicePriceDto, object?>>()
            {
                   {_localizer[_dto.GetMemberDisplayName(x=>x.Id)],item => item.Id},
{_localizer[_dto.GetMemberDisplayName(x=>x.ServiceTask)],item => item.ServiceTask},
{_localizer[_dto.GetMemberDisplayName(x=>x.Description)],item => item.Description},
{_localizer[_dto.GetMemberDisplayName(x=>x.Price)],item => item.Price}

            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);

        }
}
