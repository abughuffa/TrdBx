using CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Caching;


namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Commands.Delete;

public class DeleteDeactivateTestCaseCommand:  ICacheInvalidatorRequest<Result<int>>
{
  public int[] Id {  get; }
   public IEnumerable<string> Tags => DeactivateTestCaseCacheKey.Tags;
  public DeleteDeactivateTestCaseCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteDeactivateTestCaseCommandHandler : 
             IRequestHandler<DeleteDeactivateTestCaseCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteDeactivateTestCaseCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteDeactivateTestCaseCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteDeactivateTestCaseCommand request, CancellationToken cancellationToken)
    {
   //     await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
   //     var items = await _context.DeactivateTestCases.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
   //     foreach (var item in items)
   //     {
		 //   // raise a delete domain event
			//item.AddDomainEvent(new DeactivateTestCaseDeletedEvent(item));
   //         _context.DeactivateTestCases.Remove(item);
   //     }
   //     await _context.SaveChangesAsync(cancellationToken);
   //     return await Result.SuccessAsync();

        var items = await _context.DeactivateTestCases.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new DeactivateTestCaseDeletedEvent(item));
            _context.DeactivateTestCases.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

