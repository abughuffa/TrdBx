using CleanArchitecture.Blazor.Application.Features.ActivateTestCases.Specifications;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.Caching;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.DTOs;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.ActivateTestCases.Queries.Pagination;

public class ActivateTestCasesWithPaginationQuery : ActivateTestCaseAdvancedFilter, ICacheableRequest<PaginatedData<ActivateTestCaseDto>>
{
    public IEnumerable<string>? Tags => ActivateTestCaseCacheKey.Tags;
    public ActivateTestCaseAdvancedSpecification Specification => new(this);
    public string CacheKey => ActivateTestCaseCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Search:{Keyword}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }
}
    
public class ActivateTestCasesWithPaginationQueryHandler :
         IRequestHandler<ActivateTestCasesWithPaginationQuery, PaginatedData<ActivateTestCaseDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public ActivateTestCasesWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public ActivateTestCasesWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<PaginatedData<ActivateTestCaseDto>> Handle(ActivateTestCasesWithPaginationQuery request, CancellationToken cancellationToken)
        {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.ActivateTestCases.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //                                           .ProjectToPaginatedDataAsync<ActivateTestCase, ActivateTestCaseDto>(request.Specification,
        //                                            request.PageNumber,
        //                                            request.PageSize,
        //                                            _mapper.ConfigurationProvider,
        //                                            cancellationToken);
        //return data;

        var data = await _context.ActivateTestCases.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                     .ProjectToPaginatedDataAsync(request.Specification,
                                                                  request.PageNumber,
                                                                  request.PageSize,
                                                                  Mapper.ToDto,
                                                                  cancellationToken);
        return data;
    }
}