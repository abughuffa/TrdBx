using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Caching;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.DTOs;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Mappers;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Queries.Pagination;

public class ActivateHostingTestCasesWithPaginationQuery : ActivateHostingTestCaseAdvancedFilter, ICacheableRequest<PaginatedData<ActivateHostingTestCaseDto>>
{

    public IEnumerable<string>? Tags => ActivateHostingTestCaseCacheKey.Tags;
    public ActivateHostingTestCaseAdvancedSpecification Specification => new(this);
    public string CacheKey => ActivateHostingTestCaseCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Search:{Keyword}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }


}
    
public class ActivateHostingTestCasesWithPaginationQueryHandler :
         IRequestHandler<ActivateHostingTestCasesWithPaginationQuery, PaginatedData<ActivateHostingTestCaseDto>>
{
    ////private readonly IApplicationDbContextFactory _dbContextFactory;
    ////private readonly IMapper _mapper;
    ////public ActivateHostingTestCasesWithPaginationQueryHandler(
    ////    IApplicationDbContextFactory dbContextFactory,
    ////    IMapper mapper
    ////)
    ////{
    ////    _dbContextFactory = dbContextFactory;
    ////    _mapper = mapper;
    ////}

    private readonly IApplicationDbContext _context;
    public ActivateHostingTestCasesWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<PaginatedData<ActivateHostingTestCaseDto>> Handle(ActivateHostingTestCasesWithPaginationQuery request, CancellationToken cancellationToken)
        {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.ActivateHostingTestCases.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //                                           .ProjectToPaginatedDataAsync<ActivateHostingTestCase, ActivateHostingTestCaseDto>(request.Specification,
        //                                            request.PageNumber,
        //                                            request.PageSize,
        //                                            _mapper.ConfigurationProvider,
        //                                            cancellationToken);
        //return data;

        var data = await _context.ActivateHostingTestCases.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                     .ProjectToPaginatedDataAsync(request.Specification,
                                                                  request.PageNumber,
                                                                  request.PageSize,
                                                                  Mapper.ToDto,
                                                                  cancellationToken);
        return data;
    }
}