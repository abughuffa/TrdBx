/* using CleanArchitecture.Blazor.Application.Features.ImpulseCharts.Caching;
using CleanArchitecture.Blazor.Application.Features.ImpulseCharts.DTOs;
using CleanArchitecture.Blazor.Application.Features.ImpulseCharts.Mappers;
using CleanArchitecture.Blazor.Application.Features.ImpulseCharts.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.ImpulseCharts.Queries.Export;

public class ExportImpulseChartsQuery : ImpulseChartAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
    public ImpulseChartAdvancedSpecification Specification => new ImpulseChartAdvancedSpecification(this);
    public IEnumerable<string>? Tags => ImpulseChartCacheKey.Tags;
    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword}, {OrderBy}, {SortDirection}";
    }
    public string CacheKey => ImpulseChartCacheKey.GetExportCacheKey($"{this}");



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

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportImpulseChartsQueryHandler> _localizer;
    private readonly ImpulseChartDto _dto = new();
    public ExportImpulseChartsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportImpulseChartsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportImpulseChartsQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.ImpulseCharts.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<ImpulseChartDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await _context.ImpulseCharts.ApplySpecification(request.Specification)
              .OrderBy($"{request.OrderBy} {request.SortDirection}")
              .ProjectTo()
              .AsNoTracking()
              .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<ImpulseChartDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDescription(x=>x.ImpulseChartNo)],item => item.ImpulseChartNo},
                    {_localizer[_dto.GetMemberDescription(x=>x.ImpulseChartCode)],item => item.ImpulseChartCode},
                    {_localizer[_dto.GetMemberDescription(x=>x.VinSerNo)],item => item.VinSerNo},
                    {_localizer[_dto.GetMemberDescription(x=>x.PlateNo)],item => item.PlateNo},
                    {_localizer[_dto.GetMemberDescription(x=>x.ImpulseChartDesc)],item => item.ImpulseChartDesc},
                    {_localizer[_dto.GetMemberDescription(x=>x.IsAvaliable)],item => item.IsAvaliable},
                     {_localizer[_dto.GetMemberDescription(x=>x.OldId)],item => item.OldId},
                      {_localizer[_dto.GetMemberDescription(x=>x.OldVehicleNo)],item => item.OldVehicleNo},

            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);
    }
}


 */