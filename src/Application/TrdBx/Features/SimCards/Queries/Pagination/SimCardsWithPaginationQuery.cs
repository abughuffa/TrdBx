using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
// using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;
using CleanArchitecture.Blazor.Application.Features.SimCards.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Queries.Pagination;

public class SimCardsWithPaginationQuery : SimCardAdvancedFilter, ICacheableRequest<PaginatedData<SimCardDto>>
{
    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => SimCardCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string> Tags => SimCardCacheKey.Tags;
    public SimCardAdvancedSpecification Specification => new SimCardAdvancedSpecification(this);
}
    
public class SimCardsWithPaginationQueryHandler :
         IRequestHandler<SimCardsWithPaginationQuery, PaginatedData<SimCardDto>>
{
             private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly TypeAdapterConfig _typeAdapterConfig;
        public SimCardsWithPaginationQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory)
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
        }
    public async ValueTask<PaginatedData<SimCardDto>> Handle(SimCardsWithPaginationQuery request, CancellationToken cancellationToken)
        {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.SimCards.Include(s=>s.SPackage).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                          .ProjectToPaginatedDataAsync<SimCard, SimCardDto>(request.Specification,
                                                                       request.PageNumber,
                                                                       request.PageSize,
                                                                 _typeAdapterConfig,
                                                                       cancellationToken);
        return data;
    }
}