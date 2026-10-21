using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Caching;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Specifications;
// using CleanArchitecture.Blazor.Application.Features.ServicePrices.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Queries.GetById;

public class GetServicePriceByIdQuery : ICacheableRequest<Result<ServicePriceDto>>
{
   public required int Id { get; set; }
   public string CacheKey => ServicePriceCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string> Tags => ServicePriceCacheKey.Tags;
}

public class GetServicePriceByIdQueryHandler :
     IRequestHandler<GetServicePriceByIdQuery, Result<ServicePriceDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetServicePriceByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetServicePriceByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<ServicePriceDto>> Handle(GetServicePriceByIdQuery request, CancellationToken cancellationToken)
    {
       await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.ServicePrices.ApplySpecification(new ServicePriceByIdSpecification(request.Id))
                                .ProjectToType<ServicePriceDto>(_typeAdapterConfig)
                                  .FirstAsync(cancellationToken) ?? throw new NotFoundException($"ServicePrice with id: [{request.Id}] not found.");
        return await Result<ServicePriceDto>.SuccessAsync(data);


    }
}
