using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
// using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Queries.Export;

public class ExportTrackingUnitsQuery : TrackingUnitAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
    public TrackingUnitAdvancedSpecification Specification => new TrackingUnitAdvancedSpecification(this);
    public IEnumerable<string>? Tags => TrackingUnitCacheKey.Tags;
    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword},Client/Customer:{CustomerId},UStatus:{UStatus}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
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

    private readonly TypeAdapterConfig _typeAdapterConfig;
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportTrackingUnitsQueryHandler> _localizer;
    private readonly TrackingUnitDto _dto = new();
        public ExportTrackingUnitsQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory,
            IExcelService excelService,
            IStringLocalizer<ExportTrackingUnitsQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(ExportTrackingUnitsQuery request, CancellationToken cancellationToken)
        {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.TrackingUnits.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<TrackingUnitDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await context.TrackingUnits.ApplySpecification(request.Specification)
                    .OrderBy($"{request.OrderBy} {request.SortDirection}")
                   .ProjectToType<TrackingUnitDto>(_typeAdapterConfig)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data, new Dictionary<string, Func<TrackingUnitDto, object?>>()
            {
                   {_localizer[_dto.GetMemberDisplayName(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.SNo)],item => item.SNo},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Imei)],item => item.Imei},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.UnitName)],item => item.UnitName},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.TrackingUnitModelId)],item => item.TrackingUnitModelId},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.WryDate)],item => item.WryDate},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.TrackedAssetId)],item => item.TrackedAssetId},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.SimCardId)],item => item.SimCardId},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.CustomerId)],item => item.CustomerId},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.UStatus)],item => item.UStatus},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.IsOnWialon)],item => item.IsOnWialon},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.InsMode)],item => item.InsMode},
                     {_localizer[_dto.GetMemberDisplayName(x=>x.WUnitId)],item => item.WUnitId},
                          {_localizer[_dto.GetMemberDisplayName(x=>x.OldId)],item => item.OldId},
                    }
                    , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);

        }
}
