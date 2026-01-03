//using CleanArchitecture.Blazor.Application.Features.Charts.Dto;
//using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
//using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
//using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
//using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Specifications;

//namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Queries.Export;

//public class ExportChartItemsQuery : IRequest<Result<byte[]>>
//{

//    public ChartDto Chart { get; set; }
//    public IEnumerable<string>? Tags => TrackedAssetCacheKey.Tags;


//}


//public class ExportChartItemsQueryHandler :
//         IRequestHandler<ExportChartItemsQuery, Result<byte[]>>
//{
//    //private readonly IApplicationDbContextFactory _dbContextFactory;
//    //private readonly IMapper _mapper;
//    //private readonly IExcelService _excelService;
//    //private readonly IStringLocalizer<ExportTrackedAssetsQueryHandler> _localizer;
//    //private readonly TrackedAssetDto _dto = new();
//    //public ExportTrackedAssetsQueryHandler(
//    //    IApplicationDbContextFactory dbContextFactory,
//    //    IMapper mapper,
//    //    IExcelService excelService,
//    //    IStringLocalizer<ExportTrackedAssetsQueryHandler> localizer
//    //    )
//    //{
//    //    _dbContextFactory = dbContextFactory;
//    //    _mapper = mapper;
//    //    _excelService = excelService;
//    //    _localizer = localizer;
//    //}

//    private readonly IApplicationDbContext _context;
//    private readonly IExcelService _excelService;
//    private readonly IStringLocalizer<ExportChartItemsQueryHandler> _localizer;
//    private readonly List<string> _dto = new();
//    public ExportChartItemsQueryHandler(
//        IApplicationDbContext context,
//        IExcelService excelService,
//        IStringLocalizer<ExportChartItemsQueryHandler> localizer
//        )
//    {
//        _context = context;
//        _excelService = excelService;
//        _localizer = localizer;
//    }
//#nullable disable warnings
//    public async Task<Result<byte[]>> Handle(ExportChartItemsQuery request, CancellationToken cancellationToken)
//    {

//        var data = request.Chart.Items;
//                   //sele
//        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
//        //var data = await db.TrackedAssets.ApplySpecification(request.Specification)
//        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
//        //           .ProjectTo<TrackedAssetDto>(_mapper.ConfigurationProvider)
//        //           .AsNoTracking()
//        //           .ToListAsync(cancellationToken);

//        //var data = await _context.TrackedAssets.ApplySpecification(request.Specification)
//        //      .OrderBy($"{request.OrderBy} {request.SortDirection}")
//        //      .ProjectTo()
//        //      .AsNoTracking()
//        //      .ToListAsync(cancellationToken);


//        //var result = await _excelService.ExportAsync(data,
//        //    new Dictionary<int, string>()
//        //    {

//        //            {_localizer["Index"],item => item.s},
//        //            {_localizer[_dto.GetMemberDescription(x=>x.VinSerNo)],item => item.VinSerNo},
//        //            {_localizer[_dto.GetMemberDescription(x=>x.PlateNo)],item => item.PlateNo},
//        //            {_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetDesc)],item => item.TrackedAssetDesc},
//        //            {_localizer[_dto.GetMemberDescription(x=>x.IsAvaliable)],item => item.IsAvaliable},
//        //             {_localizer[_dto.GetMemberDescription(x=>x.OldId)],item => item.OldId},
//        //              {_localizer[_dto.GetMemberDescription(x=>x.OldVehicleNo)],item => item.OldVehicleNo},

//        //    }
//        //    , _localizer[_dto.GetClassDescription()]);

//        return await Result<byte[]>.SuccessAsync(result);
//    }
//}


