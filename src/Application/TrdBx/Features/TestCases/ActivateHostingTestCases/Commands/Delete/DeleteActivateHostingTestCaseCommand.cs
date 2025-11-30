using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Caching;


namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Commands.Delete;

public class DeleteActivateHostingTestCaseCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => ActivateHostingTestCaseCacheKey.Tags;
    public DeleteActivateHostingTestCaseCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteActivateHostingTestCaseCommandHandler :
             IRequestHandler<DeleteActivateHostingTestCaseCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteActivateHostingTestCaseCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteActivateHostingTestCaseCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteActivateHostingTestCaseCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await _context.ActivateHostingTestCases.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new ActivateHostingTestCaseDeletedEvent(item));
        //    _context.ActivateHostingTestCases.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.ActivateHostingTestCases.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new ActivateHostingTestCaseDeletedEvent(item));
            _context.ActivateHostingTestCases.Remove(item);
        }
       var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

