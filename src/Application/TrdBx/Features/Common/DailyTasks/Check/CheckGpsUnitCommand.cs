using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Check;

public class CheckTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Display(Name = "CreateDeservedServices")] public bool CreateDeservedServices { get; set; } = false;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, ActivateTrackingUnitCommand>(MemberList.None)
    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            .ForMember(dest => dest.TsDate, opt => opt.Ignore())
    //            .ForMember(dest => dest.InstallerId, opt => opt.Ignore())
    //            .ForMember(dest => dest.ApplyChangesOnWialon, opt => opt.Ignore());

    //        CreateMap<ActivateTestCase, ActivateTrackingUnitCommand>(MemberList.None)
    //      .ForMember(x => x.Id, s => s.MapFrom(y => y.TrackingUnitId));
    //      //.ForMember(x => x.InstallerId, s => s.Ignore())
    //      //.ForMember(x => x.ApplyChangesOnWialon, s => s.Ignore());
    //    }
    //}
}
public class CheckTrackingUnitCommandHandler : PriceSharedLogic, IRequestHandler<CheckTrackingUnitCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ActivateTrackingUnitCommandHandler> _localizer;
    //private readonly IWialonService _wialonService;
    //public ActivateTrackingUnitCommandHandler(IApplicationDbContextFactory dbContextFactory, 
    //                                     IStringLocalizer<ActivateTrackingUnitCommandHandler> localizer,
    //                                     IWialonService wialonService)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _wialonService = wialonService;
    //}

    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IWialonService _wialonService;

    public CheckTrackingUnitCommandHandler(
       IApplicationDbContextFactory dbContextFactory,IWialonService wialonService)
    {
       _dbContextFactory = dbContextFactory;
       _wialonService = wialonService;
    }

    public async ValueTask<Result<int>> Handle(CheckTrackingUnitCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await context.TrackingUnits.Where(x => x.Id == request.Id).Include(x => x.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync(cancellationToken) ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.InstalledActive || unit.UStatus == UStatus.InstalledActiveHosting || unit.UStatus == UStatus.InstalledActiveGprs || unit.UStatus == UStatus.InstalledInactive))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be Installed to Check it.");
        }

        var sprice = await GetSPrice(context,ServiceTask.Check);

        var serviceNo = await GenSerialNo(context, "ServiceLog", request.TsDate);

        var serviceLog = new ServiceLog()
        {
            Description = string.Format("كشف على الوحدة ({0}).", unit.SNo),
            ServiceNo = serviceNo,
            ServiceTask = ServiceTask.Check,
            CustomerId = (int)unit.CustomerId,
            SerDate = request.TsDate,
            Amount = sprice,
            IsDeserved = request.CreateDeservedServices,
            IsBilled = false,
            Subscriptions = [],
            WialonTasks = [new WialonTask (){
                TrackingUnitId = unit.Id,
                APITask = APITask.CheckOnWialon,
                Description = string.Format("تحقق من الوحدة ({0}) على منصة ويلون.", unit.SNo),
                ExcDate = request.TsDate,
               IsExecuted = false,
            }]
        };

        serviceLog.AddDomainEvent(new ServiceLogCreatedEvent(serviceLog));

        context.ServiceLogs.Add(serviceLog);

        var result = await context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return await Result<int>.SuccessAsync(unit.Id);
        }
        else
            return await Result<int>.FailureAsync("Check TrackingUnit Faild!");

    }
}

