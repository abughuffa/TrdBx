

using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Queries.GetById;

public class GetCusPriceByIdQuery : ICacheableRequest<Result<CusPriceDto>>
{
    public required int Id { get; set; }
    public string CacheKey => CusPriceCacheKey.GetByIdCacheKey($"{Id}");
    public IEnumerable<string> Tags => CusPriceCacheKey.Tags;

}

public class GetCusPriceByIdQueryHandler :
     IRequestHandler<GetCusPriceByIdQuery, Result<CusPriceDto>>
{
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetCusPriceByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<CusPriceDto>> Handle(GetCusPriceByIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.CusPrices.ApplySpecification(new CusPriceByIdSpecification(request.Id))
                                            .ProjectToType<CusPriceDto>(_typeAdapterConfig)
                                            .FirstAsync(cancellationToken);
        return await Result<CusPriceDto>.SuccessAsync(data);
    }
}
