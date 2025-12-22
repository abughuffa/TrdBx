
using CleanArchitecture.Blazor.Application.Features.POIs.DTOs;
using CleanArchitecture.Blazor.Application.Features.POIs.Caching;
using CleanArchitecture.Blazor.Application.Features.POIs.Mappers;
using CleanArchitecture.Blazor.Application.Features.POIs.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.POIs.Queries.GetById;

public class GetPOIByIdQuery : ICacheableRequest<Result<POIDto>>
{
   public required int Id { get; set; }
   public string CacheKey => POICacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string>? Tags => POICacheKey.Tags;
}

public class GetPOIByIdQueryHandler :
     IRequestHandler<GetPOIByIdQuery, Result<POIDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPOIByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<POIDto>> Handle(GetPOIByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.POIs.ApplySpecification(new POIByIdSpecification(request.Id))
                                                .ProjectTo()
                                                .FirstAsync(cancellationToken);
        return await Result<POIDto>.SuccessAsync(data);
    }
}
