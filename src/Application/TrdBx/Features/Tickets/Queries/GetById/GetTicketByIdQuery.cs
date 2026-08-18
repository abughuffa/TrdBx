using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
// using CleanArchitecture.Blazor.Application.Features.Tickets.Mappers;
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
           private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetTicketByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<TicketDto>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.Tickets.Include(t => t.TrackingUnit).ApplySpecification(new TicketByIdSpecification(request.Id))
                            .ProjectToType<TicketDto>(_typeAdapterConfig)
                              .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Ticket with id: [{request.Id}] not found.");
        return await Result<TicketDto>.SuccessAsync(data);
    }
}
