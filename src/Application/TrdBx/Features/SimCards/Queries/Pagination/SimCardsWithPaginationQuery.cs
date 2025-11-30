using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;
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
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public SimCardsWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public SimCardsWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<PaginatedData<SimCardDto>> Handle(SimCardsWithPaginationQuery request, CancellationToken cancellationToken)
        {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.SimCards.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //    .ProjectToPaginatedDataAsync<SimCard, SimCardDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
        //return data;

        var data = await _context.SimCards.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                          .ProjectToPaginatedDataAsync(request.Specification,
                                                                       request.PageNumber,
                                                                       request.PageSize,
                                                                       Mapper.ToDto,
                                                                       cancellationToken);
        return data;
    }
}