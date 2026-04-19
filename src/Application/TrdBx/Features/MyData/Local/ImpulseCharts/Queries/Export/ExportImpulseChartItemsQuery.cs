using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.DTOs;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Queries.Export;

public class ExportImpulseChartItemsQuery : ICacheableRequest<Result<byte[]>>
{
    public ImpulseChartDto ImpulseChart { get; set; } = new();

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

    //private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportImpulseChartItemsQueryHandler> _localizer;
    private readonly ItemDto _dto = new();
    public ExportImpulseChartItemsQueryHandler(
        //IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportImpulseChartItemsQueryHandler> localizer
        )
    {
        //_context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportImpulseChartItemsQuery request, CancellationToken cancellationToken)
    {
        var data = request.ImpulseChart.Items;

        if (data == null || !data.Any())
        {
            return await Result<byte[]>.FailureAsync("No data to export.");
        }   

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<ItemDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDescription(x=>x.ParentName)],item => item.ParentName},
                    {_localizer[_dto.GetMemberDescription(x=>x.ChildName)],item => item.ChildName},
                    {_localizer[_dto.GetMemberDescription(x=>x.SNo)],item => item.SNo},
                    {_localizer[_dto.GetMemberDescription(x=>x.SimNo)],item => item.SimNo},
                    {_localizer[_dto.GetMemberDescription(x=>x.Status)],item => item.Status},

            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);
    }
}


