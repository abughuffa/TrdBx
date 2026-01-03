using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
using CleanArchitecture.Blazor.Application.Features.Tickets.Specifications;
using CleanArchitecture.Blazor.Application.Features.Tickets.Mappers;
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
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public TicketsWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public TicketsWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<PaginatedData<TicketDto>> Handle(TicketsWithPaginationQuery request, CancellationToken cancellationToken)
        {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.Tickets.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //                                                          .ProjectToPaginatedDataAsync<Ticket, TicketDto>(request.Specification,
        //                                            request.PageNumber,
        //                                            request.PageSize,
        //                                            _mapper.ConfigurationProvider,
        //                                            cancellationToken);
        //return data;

        var data = await _context.Tickets.Include(s => s.TrackingUnit).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                     .ProjectToPaginatedDataAsync(request.Specification,
                                                                  request.PageNumber,
                                                                  request.PageSize,
                                                                  Mapper.ToDto,
                                                                  cancellationToken);
        return data;
    }
}