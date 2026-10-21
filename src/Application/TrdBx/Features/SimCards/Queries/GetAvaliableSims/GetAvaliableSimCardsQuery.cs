using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;
// using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;
namespace CleanArchitecture.Blazor.Application.Features.SimCards.Queries.GetAvaliableSimCards;
public class GetAvaliableSimCardsQuery : ICacheableRequest<IEnumerable<SimCardDto>>
{
    public int[]? Ids { get; set; }
    public string CacheKey => SimCardCacheKey.GetAvaliableWithIdsCacheKey($"{Ids}");
    public IEnumerable<string> Tags => SimCardCacheKey.Tags;
}
public class GetAvaliableSimCardsQueryHandler : IRequestHandler<GetAvaliableSimCardsQuery, IEnumerable<SimCardDto>>
{
          private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAvaliableSimCardsQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<IEnumerable<SimCardDto>> Handle(GetAvaliableSimCardsQuery request, CancellationToken cancellationToken)
    {

         await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

         int[] ids = request.Ids is null ? [-1] : request.Ids;

         var dataX = await _context.SimCards
            .Where(q => q.SStatus == SStatus.New || q.SStatus == SStatus.Used || ids.Contains(q.Id))
                .ProjectToType<SimCardDto>(_typeAdapterConfig)
                .ToListAsync(cancellationToken);
            
            return dataX;
}
}