using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Queries.Export;

public class ExportTrackedAssetsQuery : TrackedAssetAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
    public TrackedAssetAdvancedSpecification Specification => new TrackedAssetAdvancedSpecification(this);
    public IEnumerable<string>? Tags => TrackedAssetCacheKey.Tags;
    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword}, {OrderBy}, {SortDirection}";
    }
    public string CacheKey => TrackedAssetCacheKey.GetExportCacheKey($"{this}");



}


public class ExportTrackedAssetsQueryHandler :
         IRequestHandler<ExportTrackedAssetsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportTrackedAssetsQueryHandler> _localizer;
    //private readonly TrackedAssetDto _dto = new();
    //public ExportTrackedAssetsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportTrackedAssetsQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportTrackedAssetsQueryHandler> _localizer;
    private readonly TrackedAssetDto _dto = new();
    public ExportTrackedAssetsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportTrackedAssetsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportTrackedAssetsQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.TrackedAssets.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<TrackedAssetDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await _context.TrackedAssets.ApplySpecification(request.Specification)
              .OrderBy($"{request.OrderBy} {request.SortDirection}")
              .ProjectTo()
              .AsNoTracking()
              .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<TrackedAssetDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetNo)],item => item.TrackedAssetNo},
                    {_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetCode)],item => item.TrackedAssetCode},
                    {_localizer[_dto.GetMemberDescription(x=>x.VinSerNo)],item => item.VinSerNo},
                    {_localizer[_dto.GetMemberDescription(x=>x.PlateNo)],item => item.PlateNo},
                    {_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetDesc)],item => item.TrackedAssetDesc},
                    {_localizer[_dto.GetMemberDescription(x=>x.IsAvaliable)],item => item.IsAvaliable},
                     {_localizer[_dto.GetMemberDescription(x=>x.OldId)],item => item.OldId},
                      {_localizer[_dto.GetMemberDescription(x=>x.OldVehicleNo)],item => item.OldVehicleNo},

            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);
    }
}


