using CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;

using CleanArchitecture.Blazor.Application.Features.WialonTasks.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Queries.GetById;

public class GetWialonTaskByIdQuery : ICacheableRequest<Result<WialonTaskDto>>
{
    public required int Id { get; set; }
    public string CacheKey => WialonTaskCacheKey.GetByIdCacheKey($"{Id}");
     public IEnumerable<string> Tags => WialonTaskCacheKey.Tags;
}

public class GetWialonTaskByIdQueryHandler :
     IRequestHandler<GetWialonTaskByIdQuery, Result<WialonTaskDto>>
{
            private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetWialonTaskByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<WialonTaskDto>> Handle(GetWialonTaskByIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.WialonTasks.ApplySpecification(new WialonTaskByIdSpecification(request.Id))
                                   .ProjectToType<WialonTaskDto>(_typeAdapterConfig)
                                        .FirstAsync(cancellationToken);
        return await Result<WialonTaskDto>.SuccessAsync(data);
    }
}
