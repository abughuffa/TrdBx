using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.SimCards.Specifications;
using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;
namespace CleanArchitecture.Blazor.Application.Features.SimCards.Queries.GetAvaliableSimCards;
public class GetAvaliableSimCardsQuery : ICacheableRequest<IEnumerable<SimCardDto>>
{
    public int[]? Ids { get; set; }
    public string CacheKey => SimCardCacheKey.GetAvaliableWithIdsCacheKey($"{Ids}");
    public IEnumerable<string> Tags => SimCardCacheKey.Tags;
}
public class GetAvaliableSimCardsQueryHandler : IRequestHandler<GetAvaliableSimCardsQuery, IEnumerable<SimCardDto>>
{
    private readonly IApplicationDbContext _context;
    public GetAvaliableSimCardsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<SimCardDto>> Handle(GetAvaliableSimCardsQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.SimCards.ApplySpecification(new AvaliableSimCardSpecification(request.Ids))
                                     .ProjectTo()
                                     .ToListAsync(cancellationToken);
        return data;
    }
}
