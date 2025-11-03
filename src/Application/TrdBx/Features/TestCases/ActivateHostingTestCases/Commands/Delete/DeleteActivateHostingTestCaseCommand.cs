using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Commands.Delete;

public class DeleteActivateHostingTestCaseCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => ActivateHostingTestCaseCacheKey.Tags;
    public DeleteActivateHostingTestCaseCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteActivateHostingTestCaseCommandHandler :
             IRequestHandler<DeleteActivateHostingTestCaseCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteActivateHostingTestCaseCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(DeleteActivateHostingTestCaseCommand request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await _context.ActivateHostingTestCases.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new ActivateHostingTestCaseDeletedEvent(item));
            _context.ActivateHostingTestCases.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

