using CleanArchitecture.Blazor.Application.Features.Subscriptions.Caching;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.Mappers;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Queries.Export;

public class ExportSubscriptionsQuery : SubscriptionAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
      public SubscriptionAdvancedSpecification Specification => new SubscriptionAdvancedSpecification(this);
       public IEnumerable<string> Tags => SubscriptionCacheKey.Tags;
    public override string ToString()
    {
        return $"Search:{Keyword}, ServiceLogId: {ServiceLogId}, TrackingUnitId:{TrackingUnitId}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => SubscriptionCacheKey.GetExportCacheKey($"{this}");
}
    
public class ExportSubscriptionsQueryHandler :
         IRequestHandler<ExportSubscriptionsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportSubscriptionsQueryHandler> _localizer;
    //private readonly SubscriptionDto _dto = new() { Desc = string.Empty };
    //public ExportSubscriptionsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportSubscriptionsQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}
    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportSubscriptionsQueryHandler> _localizer;
    private readonly SubscriptionDto _dto = new();
    public ExportSubscriptionsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportSubscriptionsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }

#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportSubscriptionsQuery request, CancellationToken cancellationToken)
        {

        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.Subscriptions.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<SubscriptionDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await _context.Subscriptions.ApplySpecification(request.Specification)
    .OrderBy($"{request.OrderBy} {request.SortDirection}")
    .ProjectTo()
    .AsNoTracking()
    .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data, new Dictionary<string, Func<SubscriptionDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDescription(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDescription(x=>x.ServiceLogId)],item => item.ServiceLogId},
{_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)],item => item.TrackingUnitId},
{_localizer[_dto.GetMemberDescription(x=>x.CaseCode)],item => item.CaseCode},
{_localizer[_dto.GetMemberDescription(x=>x.LastPaidFees)],item => item.LastPaidFees},
{_localizer[_dto.GetMemberDescription(x=>x.SsDate)],item => item.SsDate},
{_localizer[_dto.GetMemberDescription(x=>x.SeDate)],item => item.SeDate},
{_localizer[_dto.GetMemberDescription(x=>x.Desc)],item => item.Desc},
{_localizer[_dto.GetMemberDescription(x=>x.DailyFees)],item => item.DailyFees},
{_localizer[_dto.GetMemberDescription(x=>x.Days)],item => item.Days},
{_localizer[_dto.GetMemberDescription(x=>x.Amount)],item => item.Amount}

                    }
                    , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);


        }
}
