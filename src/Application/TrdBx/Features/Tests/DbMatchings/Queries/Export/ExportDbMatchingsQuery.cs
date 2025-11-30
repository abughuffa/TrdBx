using CleanArchitecture.Blazor.Application.Features.DbMatchings.DTOs;
using CleanArchitecture.Blazor.Application.Features.DbMatchings.Mappers;
using CleanArchitecture.Blazor.Application.Features.DbMatchings.Specifications;



namespace CleanArchitecture.Blazor.Application.Features.DbMatchings.Queries.Export;

public class ExportDbMatchingsQuery : DbMatchingAdvancedFilter, IRequest<Result<byte[]>>
{
    public DbMatchingAdvancedSpecification Specification => new(this);


}


public class ExportDbMatchingsQueryHandler :
         IRequestHandler<ExportDbMatchingsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportDbMatchingsQueryHandler> _localizer;
    //private readonly DbMatchingDto _dto = new();
    //public ExportDbMatchingsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportDbMatchingsQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportDbMatchingsQueryHandler> _localizer;
    private readonly DbMatchingDto _dto = new();
    public ExportDbMatchingsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportDbMatchingsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportDbMatchingsQuery request, CancellationToken cancellationToken)
    {
        byte[] result;
        List<DbMatchingDto> data;
        Dictionary<string, Func<DbMatchingDto, object?>> mappers;

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        mappers = new Dictionary<string, Func<DbMatchingDto, object?>>
                {
                        {_localizer[_dto.GetMemberDescription(x=>x.Account)],item => item.Account},
                           {_localizer[_dto.GetMemberDescription(x=>x.Client)],item => item.Client},
                           {_localizer[_dto.GetMemberDescription(x=>x.Customer)],item => item.Customer},
                           {_localizer[_dto.GetMemberDescription(x=>x.TUnitSNo)],item => item.TUnitSNo},
                           {_localizer[_dto.GetMemberDescription(x=>x.WUnitSNo)],item => item.WUnitSNo},
                           {_localizer[_dto.GetMemberDescription(x=>x.TSimCardNo)],item => item.TSimCardNo},
                           {_localizer[_dto.GetMemberDescription(x=>x.WSimCardNo)],item => item.WSimCardNo},
                           {_localizer[_dto.GetMemberDescription(x=>x.StatusOnTrdBx)],item => item.StatusOnTrdBx},
                           {_localizer[_dto.GetMemberDescription(x=>x.StatusOnWialon)],item => item.StatusOnWialon},
                             {_localizer[_dto.GetMemberDescription(x=>x.WNote)],item => item.WNote},
                            {_localizer[_dto.GetMemberDescription(x=>x.TNote)],item => item.TNote}
                };

        switch (request.ListView)
        {
            case DbMatchingListView.MatchedBySimCardOnly:
                {
                   
                    data = await (from w in _context.WialonUnits
                                  join t in _context.TrackingUnits on w.SimCardNo equals t.SimCard.SimCardNo
                    where w.UnitSNo != t.SNo && w.SimCardNo != null && t.SimCard != null
                                  select new DbMatching
                                  {
                                      Account = w.Account,
                                      Client = t.Customer.Parent != null ? t.Customer.Parent.Name : null,
                                      Customer = t.Customer.Name,
                                      WUnitSNo = w.UnitSNo,
                                      TUnitSNo = t.SNo,
                                      WSimCardNo = w.SimCardNo,
                                      TSimCardNo = t.SimCard.SimCardNo,
                                      StatusOnWialon = w.StatusOnWialon,
                                      StatusOnTrdBx = t.UStatus,
                                      WNote = w.Note
                                  })
                                            .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                            .ApplySpecification(request.Specification)
                                            .AsNoTracking()
                                            .ProjectTo()
                                           .ToListAsync(cancellationToken);


                    break;
                }
            case DbMatchingListView.MatchedByUnitOnly:
                {
                    data = await (from w in _context.WialonUnits
                                  join t in _context.TrackingUnits on w.UnitSNo equals t.SNo
                                  where w.SimCardNo != t.SimCard.SimCardNo && w.UnitSNo != null && t.SimCard != null
                                  select new DbMatching
                                  {
                                      Account = w.Account,
                                      Client = t.Customer.Parent != null ? t.Customer.Parent.Name : null,
                                      Customer = t.Customer.Name,
                                      WUnitSNo = w.UnitSNo,
                                      TUnitSNo = t.SNo,
                                      WSimCardNo = w.SimCardNo,
                                      TSimCardNo = t.SimCard.SimCardNo,
                                      StatusOnWialon = w.StatusOnWialon,
                                      StatusOnTrdBx = t.UStatus,
                                      WNote = w.Note
                                  })
                                             .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                             .ApplySpecification(request.Specification)
                                             .AsNoTracking()
                                            .ProjectTo()
                                            .ToListAsync(cancellationToken);


                    break;
                }
            case DbMatchingListView.MatchedByUnitAndSimCard:
                {
                    data = await (from w in _context.WialonUnits
                                  join t in _context.TrackingUnits on w.UnitSNo equals t.SNo
                                  where w.SimCardNo == t.SimCard.SimCardNo && w.UnitSNo != null && t.SimCard != null
                                  select new DbMatching
                                  {
                                      Account = w.Account,
                                      Client = t.Customer.Parent != null ? t.Customer.Parent.Name : null,
                                      Customer = t.Customer.Name,
                                      WUnitSNo = w.UnitSNo,
                                      TUnitSNo = t.SNo,
                                      WSimCardNo = w.SimCardNo,
                                      TSimCardNo = t.SimCard.SimCardNo,
                                      StatusOnWialon = w.StatusOnWialon,
                                      StatusOnTrdBx = t.UStatus,
                                      WNote = w.Note
                                  })
                                            .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                            .ApplySpecification(request.Specification)
                                            .AsNoTracking()
                                            .ProjectTo()
                                           .ToListAsync(cancellationToken);


                    break;
                }
            default:
                {
                    data = new List<DbMatchingDto>();
                    break;
                }
        }

        result = await _excelService.ExportAsync(data, mappers, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}
