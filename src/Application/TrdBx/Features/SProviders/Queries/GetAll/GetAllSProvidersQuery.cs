using CleanArchitecture.Blazor.Application.Features.SProviders.Mappers;
using CleanArchitecture.Blazor.Application.Features.SProviders.Caching;
using CleanArchitecture.Blazor.Application.Features.SProviders.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.SProviders.Queries.GetAll;

public class GetAllSProvidersQuery : ICacheableRequest<IEnumerable<SProviderDto>>
{
   public string CacheKey => SProviderCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => SProviderCacheKey.Tags;
}

public class GetAllSProvidersQueryHandler :
     IRequestHandler<GetAllSProvidersQuery, IEnumerable<SProviderDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetAllSProvidersQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public GetAllSProvidersQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<IEnumerable<SProviderDto>> Handle(GetAllSProvidersQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.SProviders
        //    .ProjectTo<SProviderDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.SProviders.ProjectTo()
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


