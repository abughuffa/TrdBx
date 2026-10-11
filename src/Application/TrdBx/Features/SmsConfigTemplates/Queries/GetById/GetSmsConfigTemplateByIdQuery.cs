using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Caching;
using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.DTOs;
// using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Mappers;
using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Queries.GetById;

public class GetSmsConfigTemplateByIdQuery : ICacheableRequest<Result<SmsConfigTemplateDto>>
{
   public required int Id { get; set; }
   public string CacheKey => SmsConfigTemplateCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string> Tags => SmsConfigTemplateCacheKey.Tags;
}

public class GetSmsConfigTemplateByIdQueryHandler :
     IRequestHandler<GetSmsConfigTemplateByIdQuery, Result<SmsConfigTemplateDto>>
{
            private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetSmsConfigTemplateByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<SmsConfigTemplateDto>> Handle(GetSmsConfigTemplateByIdQuery request, CancellationToken cancellationToken)
    {
       await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.SmsConfigTemplates.ApplySpecification(new SmsConfigTemplateByIdSpecification(request.Id))
                           .ProjectToType<SmsConfigTemplateDto>(_typeAdapterConfig)
                           .FirstAsync(cancellationToken) ?? throw new NotFoundException($"SmsConfigTemplate with id: [{request.Id}] not found.");
        return await Result<SmsConfigTemplateDto>.SuccessAsync(data);
    }
}
