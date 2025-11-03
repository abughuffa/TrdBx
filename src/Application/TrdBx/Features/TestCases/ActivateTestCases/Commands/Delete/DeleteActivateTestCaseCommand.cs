
using CleanArchitecture.Blazor.Application.Features.ActivateTestCases.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.ActivateTestCases.Commands.Delete;

public class DeleteActivateTestCaseCommand:  ICacheInvalidatorRequest<Result>
{
  public int[] Id {  get; }
  public IEnumerable<string> Tags => ActivateTestCaseCacheKey.Tags;
  public DeleteActivateTestCaseCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteActivateTestCaseCommandHandler : 
             IRequestHandler<DeleteActivateTestCaseCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteActivateTestCaseCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(DeleteActivateTestCaseCommand request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await _context.ActivateTestCases.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
		    // raise a delete domain event
			item.AddDomainEvent(new ActivateTestCaseDeletedEvent(item));
            _context.ActivateTestCases.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

