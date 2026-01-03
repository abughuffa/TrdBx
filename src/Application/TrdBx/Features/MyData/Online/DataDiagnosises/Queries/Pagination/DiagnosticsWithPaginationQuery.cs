
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Specifications;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Mappers;
namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Queries.Pagination;


public class DataDiagnosisesWithPaginationQuery : DataDiagnosisAdvancedFilter, ICacheableRequest<PaginatedData<DataDiagnosisDto>>
{

    public IEnumerable<string>? Tags => DataDiagnosisCacheKey.Tags;
    public DataDiagnosisAdvancedSpecification Specification => new(this);
    public string CacheKey => DataDiagnosisCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword}, StatusOnWialon:{StatusOnWialon}, StatusOnTrdBx:{StatusOnTrdBx}, SimCardStatus:{SimCardStatus}, ExpiersBefore:{ExpiersBefore}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }

}

public class DataDiagnosisesWithPaginationQueryHandler : IRequestHandler<DataDiagnosisesWithPaginationQuery, PaginatedData<DataDiagnosisDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public DataDiagnosisesWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<DataDiagnosisesWithPaginationQueryHandler> _localizer;
    private readonly SimCardDto _dto = new();
    public DataDiagnosisesWithPaginationQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<DataDiagnosisesWithPaginationQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }

    public async Task<PaginatedData<DataDiagnosisDto>> Handle(DataDiagnosisesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        PaginatedData<DataDiagnosisDto> diagnostics;

        switch (request.ListView)
        {
            case DataDiagnosisListView.SimCardsOfUnitsWhichAreExistOnTrdBxAndWialon:
                {
                    diagnostics = await (from l in _context.LibyanaSimCards
                                                   join t in _context.TrackingUnits on l.SimCardNo equals t.SimCard.SimCardNo
                                                   join w in _context.WialonUnits on
                                                             new { UnitSNo = t.SNo, SimCardNo = l.SimCardNo } equals
                                                             new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo }
                                             select new DataDiagnosis
                                             {
                                                 Account = w.Account,
                                                 Client = t.Customer.Parent.Name,
                                                 Customer = t.Customer.Name,
                                                 UnitSNo = t.SNo,
                                                 SimCardNo = l.SimCardNo,
                                                 SimCardStatus = l.SimCardStatus,
                                                 StatusOnWialon = w.StatusOnWialon,
                                                 StatusOnTrdBx = t.UStatus,
                                                 WNote = w.Note,
                                                 Balance = l.Balance,
                                                 LDExDate = l.DExDate,
                                                 LDOExpired = l.DOExpired
                                             
                       }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                              .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);
                break;
                  
                }
            case DataDiagnosisListView.SimCardsOfUnitsWhichAreNotExistOnWialon:
                {
                    diagnostics = await (from l in _context.LibyanaSimCards
                                             join t in _context.TrackingUnits on l.SimCardNo equals t.SimCard.SimCardNo
                                             join w in _context.WialonUnits on
                                                 new { UnitSNo = t.SNo, SimCardNo = l.SimCardNo } equals
                                                 new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo } into wialonJoin
                                             from w in wialonJoin.DefaultIfEmpty()
                                             where w == null // This gives us records where there's no matching WialonUnit (W.SimCardNo IS NULL)
                                             select new DataDiagnosis
                                             {
                                                 Account = t.Customer.Account,
                                                 Client = t.Customer.Parent.Name,
                                                 Customer = t.Customer.Name,
                                                 UnitSNo = t.SNo,
                                                 SimCardNo = l.SimCardNo,
                                                 SimCardStatus = l.SimCardStatus,
                                                 //StatusOnWialon = w.StatusOnWialon,
                                                 StatusOnWialon = null,
                                                 StatusOnTrdBx = t.UStatus,
                                                 //WNote = w.Note,
                                                 WNote = string.Empty,
                                                 Balance = l.Balance,
                                                 LDExDate = l.DExDate,
                                                 LDOExpired = l.DOExpired

                                            }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                              .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);

                    break;
                    //return diagnostics;

                }
            case DataDiagnosisListView.SimCardsOfUnitsWhichAreNotExistOnTrdBx:
                {
                    diagnostics = await (from l in _context.LibyanaSimCards
                                             join t in _context.TrackingUnits on l.SimCardNo equals t.SimCard.SimCardNo into unitJoin
                                             from t in unitJoin.DefaultIfEmpty()
                                             join w in _context.WialonUnits on
                                                 new { UnitSNo = t.SNo, SimCardNo = l.SimCardNo } equals
                                                 new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo } into wialonJoin
                                             from w in wialonJoin.DefaultIfEmpty()
                                             where t == null // This gives us records where there's no matching Unit (T.SimCardNo IS NULL)
                                             select new DataDiagnosis
                                             {
                                                 //Account = t.Customer.Account,
                                                 //Client = t.Customer.Parent.Name,
                                                 //Customer = t.Customer.Name,
                                                 Account = w.Account,
                                                 Client = string.Empty,
                                                 Customer = string.Empty,
                                                 UnitSNo = w.UnitSNo,
                                                 SimCardNo = l.SimCardNo,
                                                 SimCardStatus = l.SimCardStatus,
                                                 StatusOnWialon = w.StatusOnWialon,
                                                 //StatusOnTrdBx = t.UStatus,
                                                 StatusOnTrdBx = null,
                                                 WNote = w.Note,
                                                 Balance = l.Balance,
                                                 LDExDate = l.DExDate,
                                                 LDOExpired = l.DOExpired

                                             }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                               .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);
                    break;
                    //return diagnostics;


                }
            case DataDiagnosisListView.SimCardsOfUnitsWhichAreNotExistOnTrdBxOrWialon:
                {

                    diagnostics = await (from l in _context.LibyanaSimCards
                                             where !_context.TrackingUnits.Any(t => t.SimCard.SimCardNo == l.SimCardNo) &&
                                                   !_context.WialonUnits.Any(w => w.SimCardNo == l.SimCardNo)
                                             select new DataDiagnosis
                                             {
                                                 Account = null,
                                                 Client = null,
                                                 Customer = null,
                                                 UnitSNo = null,
                                                 SimCardNo = l.SimCardNo,
                                                 SimCardStatus = l.SimCardStatus,
                                                 StatusOnWialon = null,
                                                 StatusOnTrdBx = null,
                                                 WNote = null,
                                                 Balance = l.Balance,
                                                 LDExDate = l.DExDate,
                                                 LDOExpired = l.DOExpired
                                             }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                             .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);

                    break;
                }
            default:
                {
                    return null;
                } 
        }

       
        return diagnostics;


    }
}

//    public static class PaginatedDataHelper
//{
//    public static async Task<PaginatedData<T>> ToPaginatedDataAsync<T>(
//        this IQueryable<T> query,
//        int pageNumber,
//        int pageSize,
//        CancellationToken cancellationToken = default)
//    {
//        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
//        pageSize = pageSize <= 0 ? 10 : pageSize;


//        var totalCount = await query.CountAsync(cancellationToken);
//        var items = await query
//            .Skip((pageNumber - 1) * pageSize)
//            .Take(pageSize)
//            .ToListAsync(cancellationToken);

//        return new PaginatedData<T>(items, totalCount, pageNumber, pageSize);
//    }
//}





