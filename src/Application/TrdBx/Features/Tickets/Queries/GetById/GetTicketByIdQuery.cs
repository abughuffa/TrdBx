using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
using CleanArchitecture.Blazor.Application.Features.Tickets.Mappers;
using CleanArchitecture.Blazor.Application.Features.Tickets.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.Queries.GetById;

public class GetTicketByIdQuery : ICacheableRequest<Result<TicketDto>>
{
   public required int Id { get; set; }
   public string CacheKey => TicketCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string> Tags => TicketCacheKey.Tags;
}

public class GetTicketByIdQueryHandler :
     IRequestHandler<GetTicketByIdQuery, Result<TicketDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetTicketByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public GetTicketByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<TicketDto>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.Tickets.ApplySpecification(new TicketByIdSpecification(request.Id))
        //                                        .ProjectTo<TicketDto>(_mapper.ConfigurationProvider)
        //                                        .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Ticket with id: [{request.Id}] not found.");
        //return await Result<TicketDto>.SuccessAsync(data);

        var data = await _context.Tickets.ApplySpecification(new TicketByIdSpecification(request.Id))
                              .ProjectTo()
                              .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Ticket with id: [{request.Id}] not found.");
        return await Result<TicketDto>.SuccessAsync(data);
    }
}
