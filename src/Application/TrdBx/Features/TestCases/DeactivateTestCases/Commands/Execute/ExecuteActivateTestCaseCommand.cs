using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.DeactivateTrackingUnit;
using CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Commands.Execute;

public class ExecuteDeactivateTestCaseCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }

    internal IMediator Mediator;
     public IEnumerable<string> Tags => DeactivateTestCaseCacheKey.Tags;
    public ExecuteDeactivateTestCaseCommand(int[] id, IMediator mediator)
    {
        Id = id;
        Mediator = mediator;
    }
}

public class ExecuteDeactivateTestCaseCommandHandler :
             IRequestHandler<ExecuteDeactivateTestCaseCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public ExecuteDeactivateTestCaseCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }
    public async Task<Result> Handle(ExecuteDeactivateTestCaseCommand request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);

        var items = await db.DeactivateTestCases.Where(x => request.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);
        if (items.Any(i => i.IsSucssed == true))
        {
            return await Result.FailureAsync("Some of selected Test cases already executed!");
        }

        foreach (var item in items)
        {



            var cmd = _mapper.Map<DeactivateTrackingUnitCommand>(item);

            var r = await request.Mediator.Send(cmd);

            item.IsSucssed = r.Succeeded;

            item.Message = r.ErrorMessage;

            item.AddDomainEvent(new DeactivateTestCaseUpdatedEvent(item));

        }

        await db.SaveChangesAsync(cancellationToken);

        return await Result.SuccessAsync();

    }

}

