//using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnit;
//using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Renew;
//using CleanArchitecture.Blazor.Application.Features.RenewTestCases.Caching;



//namespace CleanArchitecture.Blazor.Application.Features.TestCases.RenewTestCases.Commands.Execute;

//public class ExecuteRenewTestCaseCommand : ICacheInvalidatorRequest<Result>
//{
//    public int[] Id { get; }

//    internal IMediator Mediator;
//    public string CacheKey => RenewTestCaseCacheKey.GetAllCacheKey;
//    public IEnumerable<string> Tags => RenewTestCaseCacheKey.Tags;
//    public ExecuteRenewTestCaseCommand(int[] id, IMediator mediator)
//    {
//        Id = id;
//        Mediator = mediator;
//    }


//}

//public class ExecuteRenewTestCaseCommandHandler : IRequestHandler<ExecuteRenewTestCaseCommand, Result>

//{
//    private readonly IApplicationDbContext _context;

//    private readonly IMapper _mapper;
//    public ExecuteRenewTestCaseCommandHandler(
//        IMapper mapper,
//        IApplicationDbContext context)
//    {
//        _mapper = mapper;
//        _context = context;
//    }
//    public async Task<Result> Handle(ExecuteRenewTestCaseCommand request, CancellationToken cancellationToken)
//    {
//        var items = await _context.RenewTestCases.Where(x => request.Id.Contains(x.Id))
//             //.ProjectTo<RenewTestCaseDto>(_mapper.ConfigurationProvider)
//            .ToListAsync(cancellationToken);
//        if (items.Any(i => i.IsSucssed == true))
//        {
//            return await Result.FailureAsync("Some of selected Test cases already executed!");
//        }

//        foreach (var item in items)
//        {
//            var cmd = _mapper.Map<RenewTrackingUnitSubscriptionCommand>(item);

//            var r = await request.Mediator.Send(cmd);

//            //item.CaseCode = r.D
//            item.IsSucssed = r.Succeeded;

//            item.Message = r.ErrorMessage;

//            item.AddDomainEvent(new RenewTestCaseUpdatedEvent(item));
//        }

//        await _context.SaveChangesAsync(cancellationToken);

//        return await Result.SuccessAsync();

//    }

//}

