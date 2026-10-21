using CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Specifications;
using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.SimCards.Specifications;
// using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Queries.GetById;

public class GetSimCardByIdQuery : ICacheableRequest<Result<SimCardDto>>
{
   public required int Id { get; set; }
   public string CacheKey => SimCardCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string> Tags => SimCardCacheKey.Tags;
}

public class GetSimCardByIdQueryHandler :
     IRequestHandler<GetSimCardByIdQuery, Result<SimCardDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetSimCardByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetSimCardByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }    public async ValueTask<Result<SimCardDto>> Handle(GetSimCardByIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.SimCards.ApplySpecification(new SimCardByIdSpecification(request.Id))
                                .ProjectToType<SimCardDto>(_typeAdapterConfig)
                                  .FirstAsync(cancellationToken) ?? throw new NotFoundException($"SimCard with id: [{request.Id}] not found.");
        return await Result<SimCardDto>.SuccessAsync(data);

    }
}
