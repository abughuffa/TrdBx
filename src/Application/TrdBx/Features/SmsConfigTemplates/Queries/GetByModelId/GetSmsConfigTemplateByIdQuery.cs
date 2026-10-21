using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Caching;
using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.DTOs;
using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Queries.GetByModelId;

public class GetSmsConfigTemplatesByModelIdQuery : ICacheableRequest<IEnumerable<SmsConfigTemplateDto>>
{
   public required int Id { get; set; }
   public string CacheKey => SmsConfigTemplateCacheKey.GetByModelIdCacheKey($"{Id}");
   public IEnumerable<string> Tags => SmsConfigTemplateCacheKey.Tags;
}

public class GetSmsConfigTemplatesByModelIdQueryHandler  :
     IRequestHandler<GetSmsConfigTemplatesByModelIdQuery, IEnumerable<SmsConfigTemplateDto>>
{
            private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetSmsConfigTemplatesByModelIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<IEnumerable<SmsConfigTemplateDto>> Handle(GetSmsConfigTemplatesByModelIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);


        var data = await context.SmsConfigTemplates.ApplySpecification(new SmsConfigTemplateByModelIdSpecification(request.Id))
                                          .ProjectToType<SmsConfigTemplateDto>(_typeAdapterConfig)
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;

    }
}
