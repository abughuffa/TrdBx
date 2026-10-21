// using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Mappers;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;



namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Queries.Export;

public class ExportDataDiagnosisesQuery : DataDiagnosisAdvancedFilter, IRequest<Result<byte[]>>
{
    public DataDiagnosisAdvancedSpecification Specification => new(this);

}
//    public override string ToString()
//    {
//        return $"Listview:{ListView}:{LocalTimezoneOffset.TotalHours}, Search:{Keyword},StatusOnWialon:{StatusOnWialon},StatusOnTrdBx:{StatusOnTrdBx},SimCardStatus:{SimCardStatus},ExpiersBefore:{ExpiersBefore}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
//    }
//    public string CacheKey => DataDiagnosisCacheKey.GetExportCacheKey($"{this}");
//     public IEnumerable<string> Tags => DataDiagnosisCacheKey.Tags;

//    public DataDiagnosisAdvancedSpecification Specification => new DataDiagnosisAdvancedSpecification(this);
//}

public class ExportDataDiagnosisesQueryHandler :
         IRequestHandler<ExportDataDiagnosisesQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportDataDiagnosisesQueryHandler> _localizer;
    //private readonly DataDiagnosisDto _dto = new();
    //public ExportDataDiagnosisesQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportDataDiagnosisesQueryHandler> localizer
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
    private readonly IStringLocalizer<ExportDataDiagnosisesQueryHandler> _localizer;
    private readonly DataDiagnosisDto _dto = new();
        public ExportDataDiagnosisesQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory,
            IExcelService excelService,
            IStringLocalizer<ExportDataDiagnosisesQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(ExportDataDiagnosisesQuery request, CancellationToken cancellationToken)
    {
        byte[] result;
        List<DataDiagnosisDto> data;
        Dictionary<string, Func<DataDiagnosisDto, object?>> mappers;

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        mappers = new Dictionary<string, Func<DataDiagnosisDto, object?>>
                {
                       {_localizer[_dto.GetMemberDisplayName(x=>x.Account)],item => item.Account},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.Client)],item => item.Client},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.Customer)],item => item.Customer},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.UnitSNo)],item => item.UnitSNo},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.SimCardNo)],item => item.SimCardNo},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.StatusOnTrdBx)],item => item.StatusOnTrdBx},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.StatusOnWialon)],item => item.StatusOnWialon},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.SimCardStatus)],item => item.SimCardStatus},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.LDExDate)],item => item.LDExDate},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.LDOExpired)],item => item.LDOExpired},
                            {_localizer[_dto.GetMemberDisplayName(x=>x.WNote)],item => item.WNote},
                            {_localizer[_dto.GetMemberDisplayName(x=>x.Balance)],item => item.Balance}
                };

        switch (request.ListView)
        {
            case DataDiagnosisListView.SimCardsOfUnitsWhichAreExistOnTrdBxAndWialon:
                {
                    data = await (from l in context.LibyanaSimCards
                                         join u in context.TrackingUnits on l.SimCardNo equals u.SimCard.SimCardNo
                                         join w in context.WialonUnits on
                                                   new { UnitSNo = u.SNo, SimCardNo = l.SimCardNo } equals
                                                   new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo }
                                         select new DataDiagnosis
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
                                                .ProjectToType<DataDiagnosisDto>(_typeAdapterConfig)
                                                .ToListAsync(cancellationToken);
                    break;
                }
            case DataDiagnosisListView.SimCardsOfUnitsWhichAreNotExistOnWialon:
                {
                    data = await (from l in context.LibyanaSimCards
                                  join u in context.TrackingUnits on l.SimCardNo equals u.SimCard.SimCardNo
                                  join w in context.WialonUnits on
                                      new { UnitSNo = u.SNo, SimCardNo = l.SimCardNo } equals
                                      new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo } into wialonJoin
                                  from w in wialonJoin.DefaultIfEmpty()
                                  where w == null // This gives us records where there's no matching WialonUnit (W.SimCardNo IS NULL)
                                  select new DataDiagnosis
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
                                               .ProjectToType<DataDiagnosisDto>(_typeAdapterConfig)
                                                .ToListAsync(cancellationToken);

                    break;
                }
            case DataDiagnosisListView.SimCardsOfUnitsWhichAreNotExistOnTrdBx:
                {
                    data = await (from l in context.LibyanaSimCards
                                  join u in context.TrackingUnits on l.SimCardNo equals u.SimCard.SimCardNo into unitJoin
                                  from u in unitJoin.DefaultIfEmpty()
                                  join w in context.WialonUnits on
                                      new { UnitSNo = u.SNo, SimCardNo = l.SimCardNo } equals
                                      new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo } into wialonJoin
                                  from w in wialonJoin.DefaultIfEmpty()
                                  where u == null // This gives us records where there's no matching Unit (T.SimCardNo IS NULL)
                                  select new DataDiagnosis
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
                                               .ProjectToType<DataDiagnosisDto>(_typeAdapterConfig)
                                                .ToListAsync(cancellationToken);

                    break;
                }
            case DataDiagnosisListView.SimCardsOfUnitsWhichAreNotExistOnTrdBxOrWialon:
                {
                    data = await (from l in context.LibyanaSimCards
                                  where !context.TrackingUnits.Any(u => u.SimCard.SimCardNo == l.SimCardNo) &&
                                        !context.WialonUnits.Any(w => w.SimCardNo == l.SimCardNo)
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
                                                .ApplySpecification(request.Specification)
                                                .AsNoTracking()
                                                .ProjectToType<DataDiagnosisDto>(_typeAdapterConfig)
                                                .ToListAsync(cancellationToken);

                    break;
                }
            default:
                {
                    data = new List<DataDiagnosisDto>();
                    break;
                }
        }

        result = await _excelService.ExportAsync(data, mappers, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}
