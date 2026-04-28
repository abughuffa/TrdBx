using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.DTOs;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Queries.Export;

public class ExportImpulseChartsQuery : ICacheableRequest<Result<byte[]>>
{
    public List<ImpulseChartDto> ImpulseCharts { get; set; } = new();

    public IEnumerable<string>? Tags => ImpulseChartCacheKey.Tags;
    public string CacheKey => ImpulseChartCacheKey.GetExportCacheKey($"{this}");

/*       public override string ToString()
    {
        return $"ImpulseChart:{ImpulseChart.Date}";
    } */

}


public class ExportImpulseChartsQueryHandler :
         IRequestHandler<ExportImpulseChartsQuery, Result<byte[]>>
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

    //private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportImpulseChartsQueryHandler> _localizer;
    private readonly ImpulseChartDto _dto = new();
    public ExportImpulseChartsQueryHandler(
        //IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportImpulseChartsQueryHandler> localizer
        )
    {
        //_context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportImpulseChartsQuery request, CancellationToken cancellationToken)
    {
        var data = request.ImpulseCharts;

        if (data == null || !data.Any())
        {
            return await Result<byte[]>.FailureAsync("No data to export.");
        }   

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<ImpulseChartDto, object?>>()
            {
                    {_localizer["Day Of Week"],item => item.Date.DayOfWeek.ToString()},
                    {_localizer["Expairy Date"],item => item.Date.ToString("yyyy-MM-dd")},
                    {_localizer["SIMs Count"],item => item.Items.Count},
                    {_localizer["Amount"],item => (item.Items.Count)*50},
            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);
    }
}


