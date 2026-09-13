using CleanArchitecture.Blazor.Application.Features.SmsMessages.Caching;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Queries.Pagination;

public class SmsMessagesWithPaginationQuery : SmsMessageAdvancedFilter, ICacheableRequest<PaginatedData<SmsMessageDto>>
{
    public override string ToString()
    {

        return $"Listview:{ListView}, Search:{Keyword},SmsStatus:{SmsStatus}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => SmsMessageCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string> Tags => SmsMessageCacheKey.Tags;
    public SmsMessageAdvancedSpecification Specification => new SmsMessageAdvancedSpecification(this);
}

public class SmsMessagesWithPaginationQueryHandler :
         IRequestHandler<SmsMessagesWithPaginationQuery, PaginatedData<SmsMessageDto>>
{
            private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly TypeAdapterConfig _typeAdapterConfig;
        public SmsMessagesWithPaginationQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory)
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
        }
    public async ValueTask<PaginatedData<SmsMessageDto>> Handle(SmsMessagesWithPaginationQuery request, CancellationToken cancellationToken)
    {
     await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.SmsMessages.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                  .ProjectToPaginatedDataAsync<SmsMessage, SmsMessageDto>(request.Specification,
                                                                               request.PageNumber,
                                                                               request.PageSize,
                                                                               _typeAdapterConfig,
                                                                               cancellationToken);
        return data;
    }
}