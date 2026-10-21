// using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;
using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Queries.GetAll;

public class GetAllSimCardsQuery : ICacheableRequest<IEnumerable<SimCardDto>>
{
   public string CacheKey => SimCardCacheKey.GetAllCacheKey;
   public IEnumerable<string> Tags => SimCardCacheKey.Tags;
}

public class GetAllSimCardsQueryHandler :
     IRequestHandler<GetAllSimCardsQuery, IEnumerable<SimCardDto>>
{
       private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAllSimCardsQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<IEnumerable<SimCardDto>> Handle(GetAllSimCardsQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.SimCards
        //    .ProjectTo<SimCardDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.SimCards.ProjectToType<SimCardDto>(_typeAdapterConfig)
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


