using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Caching;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Specifications;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Mappers;

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

    private readonly IApplicationDbContext _context;
    public GetServicePriceByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<ServicePriceDto>> Handle(GetServicePriceByIdQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.ServicePrices.ApplySpecification(new ServicePriceByIdSpecification(request.Id))
        //                                        .ProjectTo<ServicePriceDto>(_mapper.ConfigurationProvider)
        //                                        .FirstAsync(cancellationToken) ?? throw new NotFoundException($"ServicePrice with id: [{request.Id}] not found.");
        //return await Result<ServicePriceDto>.SuccessAsync(data);


        var data = await _context.ServicePrices.ApplySpecification(new ServicePriceByIdSpecification(request.Id))
                                  .ProjectTo()
                                  .FirstAsync(cancellationToken) ?? throw new NotFoundException($"ServicePrice with id: [{request.Id}] not found.");
        return await Result<ServicePriceDto>.SuccessAsync(data);


    }
}
