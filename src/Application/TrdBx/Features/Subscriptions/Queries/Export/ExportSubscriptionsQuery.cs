using CleanArchitecture.Blazor.Application.Features.Subscriptions.Caching;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;
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
    private readonly TypeAdapterConfig _typeAdapterConfig;
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportSubscriptionsQueryHandler> _localizer;
    private readonly SubscriptionDto _dto = new();
        public ExportSubscriptionsQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory,
            IExcelService excelService,
            IStringLocalizer<ExportSubscriptionsQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(ExportSubscriptionsQuery request, CancellationToken cancellationToken)
        {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.Subscriptions.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<SubscriptionDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await context.Subscriptions.ApplySpecification(request.Specification)
    .OrderBy($"{request.OrderBy} {request.SortDirection}")
     .ProjectToType<SubscriptionDto>(_typeAdapterConfig)
    .AsNoTracking()
    .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data, new Dictionary<string, Func<SubscriptionDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.ServiceLogId)],item => item.ServiceLogId},
{_localizer[_dto.GetMemberDisplayName(x=>x.TrackingUnitId)],item => item.TrackingUnitId},
{_localizer[_dto.GetMemberDisplayName(x=>x.CaseCode)],item => item.CaseCode},
{_localizer[_dto.GetMemberDisplayName(x=>x.LastPaidFees)],item => item.LastPaidFees},
{_localizer[_dto.GetMemberDisplayName(x=>x.SsDate)],item => item.SsDate},
{_localizer[_dto.GetMemberDisplayName(x=>x.SeDate)],item => item.SeDate},
{_localizer[_dto.GetMemberDisplayName(x=>x.Description)],item => item.Description},
{_localizer[_dto.GetMemberDisplayName(x=>x.DailyFees)],item => item.DailyFees},
{_localizer[_dto.GetMemberDisplayName(x=>x.Days)],item => item.Days},
{_localizer[_dto.GetMemberDisplayName(x=>x.Amount)],item => item.Amount}

                    }
                    , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);


        }
}
