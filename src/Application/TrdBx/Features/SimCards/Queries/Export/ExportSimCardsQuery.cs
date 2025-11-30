using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;
using CleanArchitecture.Blazor.Application.Features.SimCards.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Queries.Export;

public class ExportSimCardsQuery : SimCardAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
      public SimCardAdvancedSpecification Specification => new SimCardAdvancedSpecification(this);
       public IEnumerable<string> Tags => SimCardCacheKey.Tags;
    public override string ToString()
    {
        return $"Listview:{ListView}:-{LocalTimezoneOffset.TotalHours}, Search:{Keyword}, {OrderBy}, {SortDirection}";
    }
    public string CacheKey => SimCardCacheKey.GetExportCacheKey($"{this}");

}
    
public class ExportSimCardsQueryHandler :
         IRequestHandler<ExportSimCardsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportSimCardsQueryHandler> _localizer;
    //private readonly SimCardDto _dto = new() { SimCardNo = string.Empty };
    //public ExportSimCardsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportSimCardsQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportSimCardsQueryHandler> _localizer;
    private readonly SimCardDto _dto = new();
    public ExportSimCardsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportSimCardsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportSimCardsQuery request, CancellationToken cancellationToken)
        {


        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.SimCards.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<SimCardDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await _context.SimCards.ApplySpecification(request.Specification)
    .OrderBy($"{request.OrderBy} {request.SortDirection}")
    .ProjectTo()
    .AsNoTracking()
    .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data, new Dictionary<string, Func<SimCardDto, object?>>()
            {
                   {_localizer[_dto.GetMemberDescription(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDescription(x=>x.SimCardNo)],item => item.SimCardNo},
{_localizer[_dto.GetMemberDescription(x=>x.ICCID)],item => item.ICCID},
{_localizer[_dto.GetMemberDescription(x=>x.SPackageId)],item => item.SPackageId},
{_localizer[_dto.GetMemberDescription(x=>x.SStatus)],item => item.SStatus},
{_localizer[_dto.GetMemberDescription(x=>x.ExDate)],item => item.ExDate},
 {_localizer[_dto.GetMemberDescription(x=>x.OldId)],item => item.OldId},

                    }
                    , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);

        }
}
