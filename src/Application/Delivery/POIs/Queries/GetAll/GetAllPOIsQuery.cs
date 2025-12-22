
using CleanArchitecture.Blazor.Application.Features.POIs.DTOs;
using CleanArchitecture.Blazor.Application.Features.POIs.Mappers;
using CleanArchitecture.Blazor.Application.Features.POIs.Caching;

namespace CleanArchitecture.Blazor.Application.Features.POIs.Queries.GetAll;

public class GetAllPOIsQuery : ICacheableRequest<IEnumerable<POIDto>>
{
   public string CacheKey => POICacheKey.GetAllCacheKey;
   public IEnumerable<string>? Tags => POICacheKey.Tags;
}

public class GetAllPOIsQueryHandler :
     IRequestHandler<GetAllPOIsQuery, IEnumerable<POIDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllPOIsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<POIDto>> Handle(GetAllPOIsQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.POIs.ProjectTo()
                                                .AsNoTracking()
                                                .ToListAsync(cancellationToken);
        return data;
    }
}


