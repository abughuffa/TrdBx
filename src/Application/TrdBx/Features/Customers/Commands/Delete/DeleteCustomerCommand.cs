using CleanArchitecture.Blazor.Application.Features.Customers.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Delete;

public class DeleteCustomerCommand : ICacheInvalidatorRequest<Result<int>>
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
             IRequestHandler<DeleteCustomerCommand, Result<int>>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteCustomerCommandHandler(
       IApplicationDbContextFactory dbContextFactory
    )
    {
       _dbContextFactory = dbContextFactory;
    }

    // private readonly IApplicationDbContext _context;
    // public DeleteCustomerCommandHandler(
    //     IApplicationDbContext context
    // )
    // {
    //     _context = context;
    // }
    public async ValueTask<Result<int>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await context.Customers.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new CustomerDeletedEvent(item));
            context.Customers.Remove(item);
        }
        var result = await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);

    }

}

