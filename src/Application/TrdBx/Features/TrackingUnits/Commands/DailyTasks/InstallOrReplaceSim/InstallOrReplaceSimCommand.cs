using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Application.Features.Common;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.InstallOrReplaceSim;

public class InstallOrReplaceSimCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "SimCardId")] public int SimCardId { get; set; }
    [Display(Name = "TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Display(Name = "CreateDeservedServices")] public bool CreateDeservedServices { get; set; } = false;
    [Display(Name = "ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;


    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, ReassignTrackingUnitOwnerCommand>(MemberList.None)

    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            //.ForMember(dest => dest.CustomerId, opt => opt.Ignore())
    //            .ForMember(dest => dest.ApplyChangesOnWialon, opt => opt.Ignore());
    //    }
    //}
}

public class InstallOrReplaceSimCommandHandler :PriceSharedLogic, IRequestHandler<InstallOrReplaceSimCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ReassignTrackingUnitOwnerCommandHandler> _localizer;
    //public ReassignTrackingUnitOwnerCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<ReassignTrackingUnitOwnerCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IWialonService _wialonService;

    public InstallOrReplaceSimCommandHandler(
       IApplicationDbContextFactory dbContextFactory,IWialonService wialonService)
    {
       _dbContextFactory = dbContextFactory;
       _wialonService = wialonService;
    }
    public async ValueTask<Result<int>> Handle(InstallOrReplaceSimCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var nsim = await context.SimCards.Where(x => x.Id == request.SimCardId).FirstAsync() ?? throw new NotFoundException($"SimCard with id: [{request.SimCardId}] not found.");

        if ((nsim.SStatus == SStatus.Installed || nsim.SStatus == SStatus.Recovered || nsim.SStatus == SStatus.Lost))
        {
            return await Result<int>.FailureAsync("SelectedSim Card status should be New or Used to procced");
        }

        var unit = await context.TrackingUnits.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.InstalledActive || unit.UStatus == UStatus.InstalledActiveGprs 
                    || unit.UStatus == UStatus.InstalledActiveHosting || unit.UStatus == UStatus.InstalledInactive))
        {
                        return await Result<int>.FailureAsync("Tracking Unit status should be Installed to procced");
        }

        var serviceLog = new ServiceLog()
        {
            ServiceNo = await GenSerialNo(context, "ServiceLog", request.TsDate),
            CustomerId = (int)unit.CustomerId,
            SerDate = request.TsDate,
            IsDeserved = request.CreateDeservedServices,
            IsBilled = false,
            Subscriptions = new List<Subscription>(),
            WialonTasks =  new List<WialonTask>()
        };

        if (unit.SimCardId == null)
        {
            //InstallSim  
            var serviceNo = await GenSerialNo(context, "ServiceLog", request.TsDate);

            serviceLog.Description = string.Format("تركيب شريحة الاتصال ({0}) للوحدة ({1}).", nsim.SimCardNo, unit.SNo);
            serviceLog.ServiceTask = ServiceTask.InstallSimCard;
            serviceLog.Amount = await GetSPrice(context, ServiceTask.InstallSimCard);
        }
        else
        {
            //ReplaceSim  
            var osim = await context.SimCards.Where(x => x.Id == unit.SimCardId).FirstAsync() ?? throw new NotFoundException($"SimCard with id: [{request.SimCardId}] not found.");

            osim.SStatus = SStatus.Recovered;
            osim.AddDomainEvent(new SimCardUpdatedEvent(osim));

            serviceLog.Description = string.Format("استبدال شريحة اتصال الوحدة ({0}) بالشريحة ({1}).", unit.SNo, nsim.SimCardNo);
            serviceLog.ServiceTask = ServiceTask.ReplacSimCard;
            serviceLog.Amount = await GetSPrice(context, ServiceTask.ReplacSimCard);
        }

        nsim.SStatus = SStatus.Installed;
        nsim.AddDomainEvent(new SimCardUpdatedEvent(nsim));

        unit.SimCardId = request.SimCardId;
        unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));

        if (unit.IsOnWialon)
           serviceLog.WialonTasks.Add(new WialonTask()
                    {
                        TrackingUnitId = unit.Id,
                        APITask = APITask.UpdateOnWialon,
                        Description = string.Format("حدث بيانات الوحدة ({0}) على منصة ويلون.", unit.SNo),
                        ExcDate = request.TsDate,
                        IsExecuted = false,
                    });


        serviceLog.AddDomainEvent(new ServiceLogCreatedEvent(serviceLog));

        context.ServiceLogs.Add(serviceLog);

        var result = await context.SaveChangesAsync(cancellationToken);
        if (result > 0)
        {
            if (request.ApplyChangesOnWialon)
            {
                //ExcuteRegistredTasks Here
            }
            return await Result<int>.SuccessAsync(unit.Id);
        }
        else
            return await Result<int>.FailureAsync("Installing or replacing TrackingUnit's Sim Card Faild!");


    }
}

