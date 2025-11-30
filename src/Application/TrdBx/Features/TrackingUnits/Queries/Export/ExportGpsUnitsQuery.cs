using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Queries.Export;

public class ExportTrackingUnitsQuery : TrackingUnitAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
    public TrackingUnitAdvancedSpecification Specification => new TrackingUnitAdvancedSpecification(this);
    public IEnumerable<string>? Tags => TrackingUnitCacheKey.Tags;
    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword}, {OrderBy}, {SortDirection}";
    }
    public string CacheKey => TrackingUnitCacheKey.GetExportCacheKey($"{this}");

}
    
public class ExportTrackingUnitsQueryHandler :
         IRequestHandler<ExportTrackingUnitsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportTrackingUnitsQueryHandler> _localizer;
    //private readonly TrackingUnitDto _dto = new() { SNo = string.Empty };
    //public ExportTrackingUnitsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportTrackingUnitsQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportTrackingUnitsQueryHandler> _localizer;
    private readonly TrackingUnitDto _dto = new();
    public ExportTrackingUnitsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportTrackingUnitsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportTrackingUnitsQuery request, CancellationToken cancellationToken)
        {

        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.TrackingUnits.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<TrackingUnitDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await _context.TrackingUnits.ApplySpecification(request.Specification)
                    .OrderBy($"{request.OrderBy} {request.SortDirection}")
                    .ProjectTo()
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data, new Dictionary<string, Func<TrackingUnitDto, object?>>()
            {
                   {_localizer[_dto.GetMemberDescription(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDescription(x=>x.SNo)],item => item.SNo},
                    {_localizer[_dto.GetMemberDescription(x=>x.Imei)],item => item.Imei},
                    {_localizer[_dto.GetMemberDescription(x=>x.UnitName)],item => item.UnitName},
                    {_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitModelId)],item => item.TrackingUnitModelId},
                    {_localizer[_dto.GetMemberDescription(x=>x.WryDate)],item => item.WryDate},
                    {_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetId)],item => item.TrackedAssetId},
                    {_localizer[_dto.GetMemberDescription(x=>x.SimCardId)],item => item.SimCardId},
                    {_localizer[_dto.GetMemberDescription(x=>x.CustomerId)],item => item.CustomerId},
                    {_localizer[_dto.GetMemberDescription(x=>x.UStatus)],item => item.UStatus},
                    {_localizer[_dto.GetMemberDescription(x=>x.IsOnWialon)],item => item.IsOnWialon},
                    {_localizer[_dto.GetMemberDescription(x=>x.InsMode)],item => item.InsMode},
                     {_localizer[_dto.GetMemberDescription(x=>x.WUnitId)],item => item.WUnitId},
                          {_localizer[_dto.GetMemberDescription(x=>x.OldId)],item => item.OldId},
                    }
                    , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);

        }
}
