using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.Caching;

namespace CleanArchitecture.Blazor.Application.Features.ActivateTestCases.Commands.Delete;

public class DeleteActivateTestCaseCommand:  ICacheInvalidatorRequest<Result<int>>
{
  public int[] Id {  get; }
  public IEnumerable<string> Tags => ActivateTestCaseCacheKey.Tags;
  public DeleteActivateTestCaseCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteActivateTestCaseCommandHandler : 
             IRequestHandler<DeleteActivateTestCaseCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteActivateTestCaseCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteActivateTestCaseCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteActivateTestCaseCommand request, CancellationToken cancellationToken)
    {
   //     await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
   //     var items = await _context.ActivateTestCases.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
   //     foreach (var item in items)
   //     {
		 //   // raise a delete domain event
			//item.AddDomainEvent(new ActivateTestCaseDeletedEvent(item));
   //         _context.ActivateTestCases.Remove(item);
   //     }
   //     await _context.SaveChangesAsync(cancellationToken);
   //     return await Result.SuccessAsync();

        var items = await _context.ActivateTestCases.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new ActivateTestCaseDeletedEvent(item));
            _context.ActivateTestCases.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

