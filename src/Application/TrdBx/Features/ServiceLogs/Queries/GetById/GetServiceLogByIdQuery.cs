using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
// using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Mappers;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Queries.GetById;

public class GetServiceLogByIdQuery : ICacheableRequest<Result<ServiceLogDto>>
{
   public required int Id { get; set; }
   public string CacheKey => ServiceLogCacheKey.GetByIdCacheKey($"{Id}");
    public IEnumerable<string> Tags => ServiceLogCacheKey.Tags;
}

public class GetServiceLogByIdQueryHandler :
     IRequestHandler<GetServiceLogByIdQuery, Result<ServiceLogDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetServiceLogByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetServiceLogByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<ServiceLogDto>> Handle(GetServiceLogByIdQuery request, CancellationToken cancellationToken)
    {
      await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.ServiceLogs.ApplySpecification(new ServiceLogByIdSpecification(request.Id))
                             .ProjectToType<ServiceLogDto>(_typeAdapterConfig)
                                  .FirstAsync(cancellationToken) ?? throw new NotFoundException($"ServiceLog with id: [{request.Id}] not found.");
        return await Result<ServiceLogDto>.SuccessAsync(data);

    }
}
