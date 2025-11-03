using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Delete;

public class DeleteCustomerCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }
    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => CustomerCacheKey.Tags;
    public DeleteCustomerCommand(int[] id)
    {
        Id = id;
    }

}

public class DeleteCustomerCommandHandler :
             IRequestHandler<DeleteCustomerCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteCustomerCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await db.Customers.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new CustomerDeletedEvent(item));
            db.Customers.Remove(item);
        }
        await db.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();

    }

}

