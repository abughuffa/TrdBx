using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnit;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Commands.Execute;

public class ExecuteWialonTaskCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; set; }
    public DateOnly ExcDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public bool ApplyChangesToWialon { get; set; } = true;
    public string CacheKey => WialonTaskCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => WialonTaskCacheKey.Tags;
    public ExecuteWialonTaskCommand()
    {

    }
}

public class ExecuteWialonTaskCommandHandler :
             IRequestHandler<ExecuteWialonTaskCommand, Result<int>>

{

    //private readonly IApplicationDbContextFactory __contextContextFactory;
    //private readonly IWialonService _wialonService;
    //public ExecuteWialonTaskCommandHandler(
    //    IApplicationDbContextFactory _contextContextFactory,
    //       IWialonService wialonService)
    //{
    //    __contextContextFactory = _contextContextFactory;
    //    _wialonService = wialonService;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ExecuteWialonTaskCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public ExecuteWialonTaskCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<ExecuteWialonTaskCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(ExecuteWialonTaskCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await __contextContextFactory.CreateAsync(cancellationToken);

        var item = await _context.WialonTasks.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("This is Registred Task is not exist!!");
        if (item.IsExecuted) return await Result<int>.FailureAsync("This is Registred Task is already executed!");
        if (item.ExcDate > request.ExcDate) return await Result<int>.FailureAsync("This is Registred Task is defered!");

        var oldItems = await _context.WialonTasks.Where(x =>
        x.TrackingUnitId == item.TrackingUnitId &&
        x.Id < item.Id &&
        x.IsExecuted == false &&
        !(x.ExcDate > item.ExcDate)).ToListAsync(cancellationToken);

        if (oldItems.Count != 0) return await Result<int>.FailureAsync("For this unit, Execute an older Wialon Tasks first!");


        //Execute Wialon Task

        var APIExecuationSuccessed = false;

        switch (item.APITask)
            {
                case APITask.AddToWialon:
                    {
                    var unit = _context.TrackingUnits.Include(u => u.Customer.Parent)
                        .Include(u=>u.TrackingUnitModel)
                        .Where(u => u.Id == item.TrackingUnitId).FirstAsync().Result;
                    if (request.ApplyChangesToWialon)
                        {
                            APIExecuationSuccessed = await AddToWialon(unit);
                        }
                        else
                        {
                            
                            unit.IsOnWialon = true;
                            unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
                            APIExecuationSuccessed = true;
                        }
                        break;
                    }
                case APITask.ActivateOnWialon:
                    {
                    var unit = _context.TrackingUnits.Where(u => u.Id == item.TrackingUnitId).FirstAsync().Result;
                    if (request.ApplyChangesToWialon)
                        {
                            APIExecuationSuccessed = await ActivateOnWialon(unit);
                        }
                        else
                        {
                            
                            unit.WStatus = WStatus.Active;
                            unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
                            APIExecuationSuccessed = true;
                        }
                        break;
                    }
                case APITask.DeactivateOnWialon:
                    {
                    var unit = _context.TrackingUnits.Where(u => u.Id == item.TrackingUnitId).FirstAsync().Result;
                    if (request.ApplyChangesToWialon)
                        {
                            APIExecuationSuccessed = await DeactivateOnWialon(unit);
                        }
                        else
                        {
                            
                            unit.WStatus = WStatus.Inactive;
                            unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
                            APIExecuationSuccessed = true;
                        }
                        break;
                    }
                case APITask.RemoveFromWialon:
                    {
                    var unit = _context.TrackingUnits.Where(u => u.Id == item.TrackingUnitId).FirstAsync().Result;
                    if (request.ApplyChangesToWialon)
                        {
                            APIExecuationSuccessed = await RemoveFromWialon(unit);
                        }
                        else
                        {
                           
                            unit.IsOnWialon = false;
                            unit.WStatus = null;
                            unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
                            APIExecuationSuccessed = true;
                        }
                        break;
                    }
                case APITask.UpdateOnWialon:
                {
                    var unit = _context.TrackingUnits.Include(u => u.SimCard).Where(u => u.Id == item.TrackingUnitId).FirstAsync().Result;
                   
                        if (request.ApplyChangesToWialon)
                        {
                            //unit name
                            //unit sno
                            //unit sim
                            APIExecuationSuccessed = await UpdateOnWialon(unit);
                        }
                        else
                        {
                            APIExecuationSuccessed = true;
                        }
                        break;
                    }

                default:
                    {
                        APIExecuationSuccessed = false;
                        break;
                    }
        }

        if (APIExecuationSuccessed)
        {
            item.IsExecuted = true;
            item.AddDomainEvent(new WialonTaskUpdatedEvent(item));


            var result = await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result);
        }
        else
            return await Result<int>.FailureAsync("Wialon API fiald to execute!");



    }


    private async Task<bool> AddToWialon(TrackingUnit unit)
    {
        var result = _wialonService.CreateUnit((int)unit.Customer.Parent.WUserId,unit.UnitName, unit.TrackingUnitModel.WhwTypeId, "1");

        if (result is not null) //api results
        {
            unit.IsOnWialon = true;
            //unit.WUnitId = from result
            unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
            return true;
        }
        else
            return false;
    }
    private async Task<bool> ActivateOnWialon(TrackingUnit unit)
    {

        var result = _wialonService.ActivateUnit((int)unit.WUnitId, "1");

        if (result is not null) //api results
        {
            unit.WStatus = WStatus.Active;
            unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
            return true;
        }
        else
            return false;
    }
    private async Task<bool> DeactivateOnWialon(TrackingUnit unit)
    {
        var result = _wialonService.ActivateUnit((int)unit.WUnitId, "0");

        if (result is not null) //api results
        {
            unit.WStatus = WStatus.Inactive;
            unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
            return true;
        }
        else
            return false;
    }
    private async Task<bool> RemoveFromWialon(TrackingUnit unit)
    {
        var result = _wialonService.DeleteItem((int)unit.WUnitId);

        if (result is not null) //api results
        {
            unit.IsOnWialon = false;
            unit.WStatus = null;
            unit.WUnitId = null;
            unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
            return true;
        }
        else
            return false;
    }
    private async Task<bool> UpdateOnWialon(TrackingUnit unit)
    {

        var result = _wialonService.UpdateUnitDeviceTypeUniqueId((int)unit.WUnitId, 1 ,unit.SNo);

        result = _wialonService.UpdateUnitPhoneNumber((int)unit.WUnitId, unit.SimCard.SimCardNo);

        if (result is not null) //api results
        {
            //Database Updated here
            return true;
        }
        else
            return false;
    }

}

