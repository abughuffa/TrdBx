//using CleanArchitecture.Blazor.Application.Features.RenewTestCases.Caching;
//using CleanArchitecture.Blazor.Application.Features.RenewTestCases.Specifications;
//using CleanArchitecture.Blazor.Application.Features.TestCases.RenewTestCases.DTOs;

//namespace CleanArchitecture.Blazor.Application.Features.RenewTestCases.Queries.Pagination;

//public class RenewTestCasesWithPaginationQuery : RenewTestCaseAdvancedFilter, ICacheableRequest<PaginatedData<RenewTestCaseDto>>
//{
//    public IEnumerable<string>? Tags => RenewTestCaseCacheKey.Tags;
//    public RenewTestCaseAdvancedSpecification Specification => new(this);
//    public string CacheKey => RenewTestCaseCacheKey.GetPaginationCacheKey($"{this}");
//    public override string ToString()
//    {
//        return $"Search:{Keyword}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
//    }
//}
    
//public class RenewTestCasesWithPaginationQueryHandler :
//         IRequestHandler<RenewTestCasesWithPaginationQuery, PaginatedData<RenewTestCaseDto>>
//{
//    private readonly IApplicationDbContext _context;

//    private readonly IMapper _mapper;
//    public RenewTestCasesWithPaginationQueryHandler(
//        IMapper mapper,
//        IApplicationDbContext context)
//    {
//        _mapper = mapper;
//        _context = context;
//    }

//    public async Task<PaginatedData<RenewTestCaseDto>> Handle(RenewTestCasesWithPaginationQuery request, CancellationToken cancellationToken)
//        {
//           var data = await _context.RenewTestCases.OrderBy($"{request.OrderBy} {request.SortDirection}")
//                                                   .ProjectToPaginatedDataAsync<RenewTestCase, RenewTestCaseDto>(request.Specification,
//                                                    request.PageNumber,
//                                                    request.PageSize,
//                                                    _mapper.ConfigurationProvider,
//                                                    cancellationToken);
//        return data;
//        }
//}