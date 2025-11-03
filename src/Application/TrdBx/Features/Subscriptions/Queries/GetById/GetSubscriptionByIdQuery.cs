using CleanArchitecture.Blazor.Application.Features.Subscriptions.Caching;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Queries.GetById;

public class GetSubscriptionByIdQuery : ICacheableRequest<Result<SubscriptionDto>>
{
   public required int Id { get; set; }
   public string CacheKey => SubscriptionCacheKey.GetByIdCacheKey($"{Id}");
    public IEnumerable<string> Tags => SubscriptionCacheKey.Tags;
}

public class GetSubscriptionByIdQueryHandler :
     IRequestHandler<GetSubscriptionByIdQuery, Result<SubscriptionDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public GetSubscriptionByIdQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<Result<SubscriptionDto>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await db.Subscriptions.ApplySpecification(new SubscriptionByIdSpecification(request.Id))
                                                .ProjectTo<SubscriptionDto>(_mapper.ConfigurationProvider)
                                                .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Subscription with id: [{request.Id}] not found.");
        return await Result<SubscriptionDto>.SuccessAsync(data);

    }
}
