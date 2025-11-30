using CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Caching;
using CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.DTOs;
using CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Mappers;
using CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Queries.Pagination;

public class DeactivateTestCasesWithPaginationQuery : DeactivateTestCaseAdvancedFilter, ICacheableRequest<PaginatedData<DeactivateTestCaseDto>>
{

    public override string ToString()
    {
        return $"Search:{Keyword}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => DeactivateTestCaseCacheKey.GetPaginationCacheKey($"{this}");
     public IEnumerable<string> Tags => DeactivateTestCaseCacheKey.Tags;
    public DeactivateTestCaseAdvancedSpecification Specification => new(this);
}
    
public class DeactivateTestCasesWithPaginationQueryHandler :
         IRequestHandler<DeactivateTestCasesWithPaginationQuery, PaginatedData<DeactivateTestCaseDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public DeactivateTestCasesWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public DeactivateTestCasesWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<PaginatedData<DeactivateTestCaseDto>> Handle(DeactivateTestCasesWithPaginationQuery request, CancellationToken cancellationToken)
        {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.DeactivateTestCases.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //                                         .ProjectToPaginatedDataAsync<DeactivateTestCase, DeactivateTestCaseDto>(request.Specification,
        //                                            request.PageNumber,
        //                                            request.PageSize,
        //                                            _mapper.ConfigurationProvider,
        //                                            cancellationToken);
        //return data;

        var data = await _context.DeactivateTestCases.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                     .ProjectToPaginatedDataAsync(request.Specification,
                                                                  request.PageNumber,
                                                                  request.PageSize,
                                                                  Mapper.ToDto,
                                                                  cancellationToken);
        return data;
    }
}