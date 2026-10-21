using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Caching;
using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.DTOs;
    // using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Mappers;
using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Queries.Pagination;

public class SmsConfigTemplatesWithPaginationQuery : SmsConfigTemplateAdvancedFilter, ICacheableRequest<PaginatedData<SmsConfigTemplateDto>>
{

    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword},TrackingUnitModel: {TrackingUnitModelId}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }

    public string CacheKey => SmsConfigTemplateCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => SmsConfigTemplateCacheKey.Tags;
    public SmsConfigTemplateAdvancedSpecification Specification => new(this);

}
    
public class SmsConfigTemplatesWithPaginationQueryHandler :
         IRequestHandler<SmsConfigTemplatesWithPaginationQuery, PaginatedData<SmsConfigTemplateDto>>
{
            private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly TypeAdapterConfig _typeAdapterConfig;
        public SmsConfigTemplatesWithPaginationQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory)
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
        }

    public async ValueTask<PaginatedData<SmsConfigTemplateDto>> Handle(SmsConfigTemplatesWithPaginationQuery request, CancellationToken cancellationToken)
        {
         await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.SmsConfigTemplates.Include(s => s.TrackingUnitModel).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                     .ProjectToPaginatedDataAsync<SmsConfigTemplate, SmsConfigTemplateDto>(request.Specification,
                                                                  request.PageNumber,
                                                                  request.PageSize,
                                                               _typeAdapterConfig,
                                                                  cancellationToken);
        return data;
    }
}