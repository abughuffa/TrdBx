using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Caching;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.DTOs;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Mappers;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Queries.Pagination;

public class ActivateGprsTestCasesWithPaginationQuery : ActivateGprsTestCaseAdvancedFilter, ICacheableRequest<PaginatedData<ActivateGprsTestCaseDto>>
{
    public IEnumerable<string>? Tags => ActivateGprsTestCaseCacheKey.Tags;
    public ActivateGprsTestCaseAdvancedSpecification Specification => new(this);
    public string CacheKey => ActivateGprsTestCaseCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Search:{Keyword}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }

}
    
public class ActivateGprsTestCasesWithPaginationQueryHandler :
         IRequestHandler<ActivateGprsTestCasesWithPaginationQuery, PaginatedData<ActivateGprsTestCaseDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public ActivateGprsTestCasesWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public ActivateGprsTestCasesWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<PaginatedData<ActivateGprsTestCaseDto>> Handle(ActivateGprsTestCasesWithPaginationQuery request, CancellationToken cancellationToken)
        {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.ActivateGprsTestCases.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //                                         .ProjectToPaginatedDataAsync<ActivateGprsTestCase, ActivateGprsTestCaseDto>(request.Specification,
        //                                            request.PageNumber,
        //                                            request.PageSize,
        //                                            _mapper.ConfigurationProvider,
        //                                            cancellationToken);
        //return data;

        var data = await _context.ActivateGprsTestCases.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                     .ProjectToPaginatedDataAsync(request.Specification,
                                                                  request.PageNumber,
                                                                  request.PageSize,
                                                                  Mapper.ToDto,
                                                                  cancellationToken);
        return data;
    }
}