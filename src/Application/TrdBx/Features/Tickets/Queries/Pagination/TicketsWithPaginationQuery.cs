using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
using CleanArchitecture.Blazor.Application.Features.Tickets.Specifications;
// using CleanArchitecture.Blazor.Application.Features.Tickets.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.Queries.Pagination;

public class TicketsWithPaginationQuery : TicketAdvancedFilter, ICacheableRequest<PaginatedData<TicketDto>>
{

    public IEnumerable<string>? Tags => TicketCacheKey.Tags;
    public TicketAdvancedSpecification Specification => new(this);
    public string CacheKey => TicketCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword}, ServiceTask:{ServiceTask}, TicketStatus:{TicketStatus}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }


}
    
public class TicketsWithPaginationQueryHandler :
         IRequestHandler<TicketsWithPaginationQuery, PaginatedData<TicketDto>>
{
            private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly TypeAdapterConfig _typeAdapterConfig;
        public TicketsWithPaginationQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory)
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
        }

    public async ValueTask<PaginatedData<TicketDto>> Handle(TicketsWithPaginationQuery request, CancellationToken cancellationToken)
        {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.Tickets.Include(s => s.TrackingUnit).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                     .ProjectToPaginatedDataAsync<Ticket, TicketDto>(request.Specification,
                                                                  request.PageNumber,
                                                                  request.PageSize,
                                                             _typeAdapterConfig,
                                                                  cancellationToken);
        return data;
    }
}