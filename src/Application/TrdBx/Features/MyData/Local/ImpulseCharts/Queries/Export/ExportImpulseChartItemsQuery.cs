using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.DTOs;


namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Queries.Export;

public class ExportImpulseChartItemsQuery : ICacheableRequest<Result<byte[]>>
{
    public Impulse ImpulseChart { get; set; } = new();

    public IEnumerable<string>? Tags => ImpulseChartCacheKey.Tags;
    public string CacheKey => ImpulseChartCacheKey.GetExportCacheKey($"{this}");

/*       public override string ToString()
    {
        return $"ImpulseChart:{ImpulseChart.Date}";
    } */

}


public class ExportImpulseChartItemsQueryHandler :
         IRequestHandler<ExportImpulseChartItemsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportImpulseChartsQueryHandler> _localizer;
    //private readonly ImpulseChartDto _dto = new();
    //public ExportImpulseChartsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportImpulseChartsQueryHandler> localizer
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
    private readonly IStringLocalizer<ExportImpulseChartItemsQueryHandler> _localizer;
    private readonly ExpiryObject _dto = new();
        public ExportImpulseChartItemsQueryHandler(
            TypeAdapterConfig typeAdapterConfig,

            IExcelService excelService,
            IStringLocalizer<ExportImpulseChartItemsQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;

            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(ExportImpulseChartItemsQuery request, CancellationToken cancellationToken)
    {
        var data = request.ImpulseChart.ExpiryObjects;

        if (data == null || !data.Any())
        {
            return await Result<byte[]>.FailureAsync("No data to export.");
        }   

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<ExpiryObject, object?>>()
            {
                    {_localizer[_dto.GetMemberDisplayName(x=>x.CustomerName)],item => item.CustomerName},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.ExDate)],item => item.ExDate},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.SNo)],item => item.SNo},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.SimNo)],item => item.SimNo},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Status)],item => item.Status},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.DaysRemaining)],item => item.DaysRemaining},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.ObjectStatus)],item => item.ObjectStatus}

            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);
    }
}


