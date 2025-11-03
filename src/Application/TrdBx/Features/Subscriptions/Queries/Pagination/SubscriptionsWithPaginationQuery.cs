using CleanArchitecture.Blazor.Application.Features.Subscriptions.Caching;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Queries.Pagination;

public class SubscriptionsWithPaginationQuery : SubscriptionAdvancedFilter, ICacheableRequest<PaginatedData<SubscriptionDto>>
{
    public override string ToString()
    {
        return $"Search:{Keyword}, ServiceLogId: {ServiceLogId}, TrackingUnitId:{TrackingUnitId}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => SubscriptionCacheKey.GetPaginationCacheKey($"{this}");
     public IEnumerable<string> Tags => SubscriptionCacheKey.Tags;
    public SubscriptionAdvancedSpecification Specification => new SubscriptionAdvancedSpecification(this);
}
    
public class SubscriptionsWithPaginationQueryHandler :
         IRequestHandler<SubscriptionsWithPaginationQuery, PaginatedData<SubscriptionDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public SubscriptionsWithPaginationQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<PaginatedData<SubscriptionDto>> Handle(SubscriptionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await db.Subscriptions.OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<Subscription, SubscriptionDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
        return data;

        }
}