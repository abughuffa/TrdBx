using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnitForHosting;
using CleanArchitecture.Blazor.Domain.Events;



namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Commands.Execute;

public class ExecuteActivateHostingTestCaseCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }

    internal IMediator Mediator;
     public IEnumerable<string> Tags => ActivateHostingTestCaseCacheKey.Tags;
    public ExecuteActivateHostingTestCaseCommand(int[] id, IMediator mediator)
    {
        Id = id;
        Mediator = mediator;
    }




}

public class ExecuteActivateHostingTestCaseCommandHandler :
             IRequestHandler<ExecuteActivateHostingTestCaseCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public ExecuteActivateHostingTestCaseCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }
    public async Task<Result> Handle(ExecuteActivateHostingTestCaseCommand request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);

        var items = await db.ActivateHostingTestCases.Where(x => request.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);
        if (items.Any(i => i.IsSucssed == true))
        {
            return await Result.FailureAsync("Some of selected Test cases already executed!");
        }

        foreach (var item in items)
        {

            var cmd = _mapper.Map<ActivateTrackingUnitForHostingCommand>(item);

            var r = await request.Mediator.Send(cmd);

            item.IsSucssed = r.Succeeded;

            item.Message = r.ErrorMessage;

            item.AddDomainEvent(new ActivateHostingTestCaseUpdatedEvent(item));

        }

        await db.SaveChangesAsync(cancellationToken);

        return await Result.SuccessAsync();

    }

}

