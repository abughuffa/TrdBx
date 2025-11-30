using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;
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
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetAllSimCardsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetAllSimCardsQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<IEnumerable<SimCardDto>> Handle(GetAllSimCardsQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.SimCards
        //    .ProjectTo<SimCardDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.SimCards.ProjectTo()
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


