using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
using CleanArchitecture.Blazor.Application.Features.Tickets.Mappers;
using CleanArchitecture.Blazor.Application.Features.Tickets.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.Queries.Export;

public class ExportTicketsQuery : TicketAdvancedFilter, IRequest<Result<byte[]>>
{
    public TicketAdvancedSpecification Specification => new(this);

}
    
public class ExportTicketsQueryHandler :
         IRequestHandler<ExportTicketsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportTicketsQueryHandler> _localizer;
    //private readonly TicketDto _dto = new();
    //public ExportTicketsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportTicketsQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}
    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportTicketsQueryHandler> _localizer;
    private readonly TicketDto _dto = new();
    public ExportTicketsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportTicketsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportTicketsQuery request, CancellationToken cancellationToken)
        {
    //    await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
    //    var data = await _context.Tickets.ApplySpecification(request.Specification)
    //.AsNoTracking()
    //.ProjectTo<TicketDto>(_mapper.ConfigurationProvider)
    //.ToListAsync(cancellationToken);

        var data = await _context.Tickets.ApplySpecification(request.Specification)
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectTo()
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        byte[] result;
        Dictionary<string, Func<TicketDto, object?>> mappers;

        mappers = new Dictionary<string, Func<TicketDto, object?>>
                {
                    {_localizer[_dto.GetMemberDescription(x=>x.TicketNo)],item => item.TicketNo},
                    {_localizer[_dto.GetMemberDescription(x=>x.ServiceTask)],item => item.ServiceTask},
                    {_localizer[_dto.GetMemberDescription(x=>x.Desc)],item => item.Desc},
                    {_localizer[_dto.GetMemberDescription(x=>x.TicketStatus)],item => item.TicketStatus},
                    {_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)],item => item.TrackingUnitId},
                    {_localizer[_dto.GetMemberDescription(x=>x.TcDate)],item => item.TcDate},
                     {_localizer[_dto.GetMemberDescription(x=>x.TaDate)],item => item.TaDate},
                      //{_localizer[_dto.GetMemberDescription(x=>x.InstallerId)],item => item.InstallerId},
                       {_localizer[_dto.GetMemberDescription(x=>x.TeDate)],item => item.TeDate},
                        {_localizer[_dto.GetMemberDescription(x=>x.Note)],item => item.Note}
                };

        result = await _excelService.ExportAsync(data, mappers, _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);

        }
}
