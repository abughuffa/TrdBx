using CleanArchitecture.Blazor.Application.Features.Subscriptions.Caching;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;
    // using CleanArchitecture.Blazor.Application.Features.Subscriptions.Mappers;
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
        private readonly TypeAdapterConfig _typeAdapterConfig;
        public SubscriptionsWithPaginationQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory)
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
        }
    public async ValueTask<PaginatedData<SubscriptionDto>> Handle(SubscriptionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
         await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.Subscriptions.Include(s => s.ServiceLog).Include(s => s.TrackingUnit).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                          .ProjectToPaginatedDataAsync<Subscription, SubscriptionDto>(request.Specification,
                                                                       request.PageNumber,
                                                                       request.PageSize,
                                                                   _typeAdapterConfig,
                                                                       cancellationToken);
        return data;

    }
}