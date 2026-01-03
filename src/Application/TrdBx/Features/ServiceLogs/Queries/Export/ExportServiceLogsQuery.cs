using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Mappers;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Queries.Export;

public class ExportServiceLogsQuery : ServiceLogAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
      public ServiceLogAdvancedSpecification Specification => new ServiceLogAdvancedSpecification(this);
       public IEnumerable<string> Tags => ServiceLogCacheKey.Tags;
    public override string ToString()
    {
        return
            $"ListView:{ListView}, Search:{Keyword}, IsBilled: {IsBilled}, Client/Customer: {CustomerId}, ServiceTask: {ServiceTask}, SortDirection: {SortDirection}, OrderBy: {OrderBy}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => ServiceLogCacheKey.GetExportCacheKey($"{this}");
}
    
public class ExportServiceLogsQueryHandler :
         IRequestHandler<ExportServiceLogsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportServiceLogsQueryHandler> _localizer;
    //private readonly ServiceLogDto _dto = new() { Desc = string.Empty, InstallerId = string.Empty };
    //public ExportServiceLogsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportServiceLogsQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportServiceLogsQueryHandler> _localizer;
    private readonly ServiceLogDto _dto = new();
    public ExportServiceLogsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportServiceLogsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportServiceLogsQuery request, CancellationToken cancellationToken)
        {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.ServiceLogs.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<ServiceLogDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await _context.ServiceLogs.ApplySpecification(request.Specification)
    .OrderBy($"{request.OrderBy} {request.SortDirection}")
    .ProjectTo()
    .AsNoTracking()
    .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<ServiceLogDto, object?>>()
            {
                     {_localizer[_dto.GetMemberDescription(x=>x.Id)],item => item.Id},
{_localizer[_dto.GetMemberDescription(x=>x.ServiceNo)],item => item.ServiceNo},
{_localizer[_dto.GetMemberDescription(x=>x.ServiceTask)],item => item.ServiceTask},
{_localizer[_dto.GetMemberDescription(x=>x.CustomerId)],item => item.CustomerId},
//{_localizer[_dto.GetMemberDescription(x=>x.InstallerId)],item => item.InstallerId},
{_localizer[_dto.GetMemberDescription(x=>x.Desc)],item => item.Desc},

{_localizer[_dto.GetMemberDescription(x=>x.SerDate)],item => item.SerDate},
                {_localizer[_dto.GetMemberDescription(x=>x.IsDeserved)],item => item.IsDeserved},
                 {_localizer[_dto.GetMemberDescription(x=>x.IsBilled)],item => item.IsBilled},
                {_localizer[_dto.GetMemberDescription(x=>x.Amount)],item => item.Amount}

            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);

        }
}


