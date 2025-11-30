using CleanArchitecture.Blazor.Application.Features.Diagnostics.DTOs;
using CleanArchitecture.Blazor.Application.Features.Diagnostics.Mappers;
using CleanArchitecture.Blazor.Application.Features.Diagnostics.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;



namespace CleanArchitecture.Blazor.Application.Features.Diagnostics.Queries.Export;

public class ExportDiagnosticsQuery : DiagnosticAdvancedFilter, IRequest<Result<byte[]>>
{
    public DiagnosticAdvancedSpecification Specification => new(this);

}
//    public override string ToString()
//    {
//        return $"Listview:{ListView}:{LocalTimezoneOffset.TotalHours}, Search:{Keyword},StatusOnWialon:{StatusOnWialon},StatusOnTrdBx:{StatusOnTrdBx},SimCardStatus:{SimCardStatus},ExpiersBefore:{ExpiersBefore}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
//    }
//    public string CacheKey => DiagnosticCacheKey.GetExportCacheKey($"{this}");
//     public IEnumerable<string> Tags => DiagnosticCacheKey.Tags;

//    public DiagnosticAdvancedSpecification Specification => new DiagnosticAdvancedSpecification(this);
//}

public class ExportDiagnosticsQueryHandler :
         IRequestHandler<ExportDiagnosticsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportDiagnosticsQueryHandler> _localizer;
    //private readonly DiagnosticDto _dto = new();
    //public ExportDiagnosticsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportDiagnosticsQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportDiagnosticsQueryHandler> _localizer;
    private readonly DiagnosticDto _dto = new();
    public ExportDiagnosticsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportDiagnosticsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportDiagnosticsQuery request, CancellationToken cancellationToken)
    {
        byte[] result;
        List<DiagnosticDto> data;
        Dictionary<string, Func<DiagnosticDto, object?>> mappers;

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        mappers = new Dictionary<string, Func<DiagnosticDto, object?>>
                {
                       {_localizer[_dto.GetMemberDescription(x=>x.Account)],item => item.Account},
                           {_localizer[_dto.GetMemberDescription(x=>x.Client)],item => item.Client},
                           {_localizer[_dto.GetMemberDescription(x=>x.Customer)],item => item.Customer},
                           {_localizer[_dto.GetMemberDescription(x=>x.UnitSNo)],item => item.UnitSNo},
                           {_localizer[_dto.GetMemberDescription(x=>x.SimCardNo)],item => item.SimCardNo},
                           {_localizer[_dto.GetMemberDescription(x=>x.StatusOnTrdBx)],item => item.StatusOnTrdBx},
                           {_localizer[_dto.GetMemberDescription(x=>x.StatusOnWialon)],item => item.StatusOnWialon},
                           {_localizer[_dto.GetMemberDescription(x=>x.SimCardStatus)],item => item.SimCardStatus},
                           {_localizer[_dto.GetMemberDescription(x=>x.LDExDate)],item => item.LDExDate},
                           {_localizer[_dto.GetMemberDescription(x=>x.LDOExpired)],item => item.LDOExpired},
                            {_localizer[_dto.GetMemberDescription(x=>x.WNote)],item => item.WNote},
                            {_localizer[_dto.GetMemberDescription(x=>x.Balance)],item => item.Balance}
                };

        switch (request.ListView)
        {
            case DiagnosticListView.SimCardsOfUnitsWhichAreExistOnTrdBxAndWialon:
                {
                    data = await (from l in _context.LibyanaSimCards
                                         join u in _context.TrackingUnits on l.SimCardNo equals u.SimCard.SimCardNo
                                         join w in _context.WialonUnits on
                                                   new { UnitSNo = u.SNo, SimCardNo = l.SimCardNo } equals
                                                   new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo }
                                         select new Diagnostic
                                         {
                                             Account = w.Account,
                                             Client = u.Customer.Parent.Name,
                                             Customer = u.Customer.Name,
                                             UnitSNo = u.SNo,
                                             SimCardNo = l.SimCardNo,
                                             SimCardStatus = l.SimCardStatus,
                                             StatusOnWialon = w.StatusOnWialon,
                                             StatusOnTrdBx = u.UStatus,
                                             WNote = w.Note,
                                             Balance = l.Balance,
                                             LDExDate = l.DExDate,
                                             LDOExpired = l.DOExpired
                                         })
                      .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                            .ApplySpecification(request.Specification)
                                                 .AsNoTracking()
                                                .ProjectTo()
                                                .ToListAsync(cancellationToken);
                    break;
                }
            case DiagnosticListView.SimCardsOfUnitsWhichAreNotExistOnWialon:
                {
                    data = await (from l in _context.LibyanaSimCards
                                  join u in _context.TrackingUnits on l.SimCardNo equals u.SimCard.SimCardNo
                                  join w in _context.WialonUnits on
                                      new { UnitSNo = u.SNo, SimCardNo = l.SimCardNo } equals
                                      new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo } into wialonJoin
                                  from w in wialonJoin.DefaultIfEmpty()
                                  where w == null // This gives us records where there's no matching WialonUnit (W.SimCardNo IS NULL)
                                  select new Diagnostic
                                  {
                                      Account = u.Customer.Account,
                                      Client = u.Customer.Parent.Name,
                                      Customer = u.Customer.Name,
                                      UnitSNo = u.SNo,
                                      SimCardNo = l.SimCardNo,
                                      SimCardStatus = l.SimCardStatus,
                                      StatusOnWialon = w.StatusOnWialon,
                                      StatusOnTrdBx = u.UStatus,
                                      WNote = w.Note,
                                      Balance = l.Balance,
                                      LDExDate = l.DExDate,
                                      LDOExpired = l.DOExpired

                                  }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                    .ApplySpecification(request.Specification)
                                                    .AsNoTracking()
                                                 .ProjectTo()
                                                .ToListAsync(cancellationToken);

                    break;
                }
            case DiagnosticListView.SimCardsOfUnitsWhichAreNotExistOnTrdBx:
                {
                    data = await (from l in _context.LibyanaSimCards
                                  join u in _context.TrackingUnits on l.SimCardNo equals u.SimCard.SimCardNo into unitJoin
                                  from u in unitJoin.DefaultIfEmpty()
                                  join w in _context.WialonUnits on
                                      new { UnitSNo = u.SNo, SimCardNo = l.SimCardNo } equals
                                      new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo } into wialonJoin
                                  from w in wialonJoin.DefaultIfEmpty()
                                  where u == null // This gives us records where there's no matching Unit (T.SimCardNo IS NULL)
                                  select new Diagnostic
                                  {
                                      Account = u.Customer.Account,
                                      Client = u.Customer.Parent.Name,
                                      Customer = u.Customer.Name,
                                      UnitSNo = u.SNo,
                                      SimCardNo = l.SimCardNo,
                                      SimCardStatus = l.SimCardStatus,
                                      StatusOnWialon = w.StatusOnWialon,
                                      StatusOnTrdBx = u.UStatus,
                                      WNote = w.Note,
                                      Balance = l.Balance,
                                      LDExDate = l.DExDate,
                                      LDOExpired = l.DOExpired
                                  }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                .ApplySpecification(request.Specification)
                                                .AsNoTracking()
                                                .ProjectTo()
                                                .ToListAsync(cancellationToken);

                    break;
                }
            case DiagnosticListView.SimCardsOfUnitsWhichAreNotExistOnTrdBxOrWialon:
                {
                    data = await (from l in _context.LibyanaSimCards
                                  where !_context.TrackingUnits.Any(u => u.SimCard.SimCardNo == l.SimCardNo) &&
                                        !_context.WialonUnits.Any(w => w.SimCardNo == l.SimCardNo)
                                  select new Diagnostic
                                  {
                                      Account = null,
                                      Client = null,
                                      Customer = null,
                                      UnitSNo = null,
                                      SimCardNo = l.SimCardNo,
                                      SimCardStatus = l.SimCardStatus,
                                      StatusOnWialon = WStatus.Null,
                                      StatusOnTrdBx = UStatus.Null,
                                      WNote = null,
                                      Balance = l.Balance,
                                      LDExDate = l.DExDate,
                                      LDOExpired = l.DOExpired
                                  }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                .ApplySpecification(request.Specification)
                                                .AsNoTracking()
                                                 .ProjectTo()
                                                .ToListAsync(cancellationToken);

                    break;
                }
            default:
                {
                    data = new List<DiagnosticDto>();
                    break;
                }
        }

        result = await _excelService.ExportAsync(data, mappers, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}
