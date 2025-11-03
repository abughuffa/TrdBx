using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.SimCards.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Queries.GetAvaliableSimCards;

public class GetAvaliableSimCardsQuery : ICacheableRequest<IEnumerable<SimCardDto>>
{
    public int[]? Ids { get; set; }
    public string CacheKey => SimCardCacheKey.GetAvaliableWithIdsCacheKey($"{Ids}");
     public IEnumerable<string> Tags => SimCardCacheKey.Tags;
}

public class GetAvaliableSimCardsQueryHandler :
     IRequestHandler<GetAvaliableSimCardsQuery, IEnumerable<SimCardDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public GetAvaliableSimCardsQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SimCardDto>> Handle(GetAvaliableSimCardsQuery request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);


            var data = await db.SimCards.ApplySpecification(new AvaliableSimCardSpecification(request.Ids))
                                        .ProjectTo<SimCardDto>(_mapper.ConfigurationProvider)
                                        .ToListAsync(cancellationToken);
            return data;

    }
}
