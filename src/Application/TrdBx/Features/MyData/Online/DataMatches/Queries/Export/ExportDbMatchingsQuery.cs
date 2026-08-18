
// using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.Mappers;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.Specifications;



namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.Queries.Export;

public class ExportDataMatchesQuery : DataMatchAdvancedFilter, IRequest<Result<byte[]>>
{
    public DataMatchAdvancedSpecification Specification => new(this);


}


public class ExportDataMatchesQueryHandler :
         IRequestHandler<ExportDataMatchesQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportDataMatchesQueryHandler> _localizer;
    //private readonly DataMatchDto _dto = new();
    //public ExportDataMatchesQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportDataMatchesQueryHandler> localizer
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
    private readonly IStringLocalizer<ExportDataMatchesQueryHandler> _localizer;
    private readonly DataMatchDto _dto = new();
        public ExportDataMatchesQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory,
            IExcelService excelService,
            IStringLocalizer<ExportDataMatchesQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(ExportDataMatchesQuery request, CancellationToken cancellationToken)
    {
        byte[] result;
        List<DataMatchDto> data;
        Dictionary<string, Func<DataMatchDto, object?>> mappers;

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        mappers = new Dictionary<string, Func<DataMatchDto, object?>>
                {
                        {_localizer[_dto.GetMemberDisplayName(x=>x.Account)],item => item.Account},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.Client)],item => item.Client},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.Customer)],item => item.Customer},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.TUnitSNo)],item => item.TUnitSNo},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.WUnitSNo)],item => item.WUnitSNo},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.TSimCardNo)],item => item.TSimCardNo},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.WSimCardNo)],item => item.WSimCardNo},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.StatusOnTrdBx)],item => item.StatusOnTrdBx},
                           {_localizer[_dto.GetMemberDisplayName(x=>x.StatusOnWialon)],item => item.StatusOnWialon},
                             {_localizer[_dto.GetMemberDisplayName(x=>x.WNote)],item => item.WNote}
                };

        switch (request.ListView)
        {
            case DataMatchListView.MatchedBySimCardOnly:
                {
                   
                    data = await (from w in context.WialonUnits
                                  join t in context.TrackingUnits on w.SimCardNo equals t.SimCard.SimCardNo
                    where w.UnitSNo != t.SNo && w.SimCardNo != null && t.SimCard != null
                                  select new DataMatch
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
                                             .ProjectToType<DataMatchDto>(_typeAdapterConfig)
                                           .ToListAsync(cancellationToken);


                    break;
                }
            case DataMatchListView.MatchedByUnitOnly:
                {
                    data = await (from w in context.WialonUnits
                                  join t in context.TrackingUnits on w.UnitSNo equals t.SNo
                                  where w.SimCardNo != t.SimCard.SimCardNo && w.UnitSNo != null && t.SimCard != null
                                  select new DataMatch
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
                                             .ProjectToType<DataMatchDto>(_typeAdapterConfig)
                                            .ToListAsync(cancellationToken);


                    break;
                }
            case DataMatchListView.MatchedByUnitAndSimCard:
                {
                    data = await (from w in context.WialonUnits
                                  join t in context.TrackingUnits on w.UnitSNo equals t.SNo
                                  where w.SimCardNo == t.SimCard.SimCardNo && w.UnitSNo != null && t.SimCard != null
                                  select new DataMatch
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
                                            .ProjectToType<DataMatchDto>(_typeAdapterConfig)
                                           .ToListAsync(cancellationToken);


                    break;
                }
            default:
                {
                    data = new List<DataMatchDto>();
                    break;
                }
        }

        result = await _excelService.ExportAsync(data, mappers, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}
