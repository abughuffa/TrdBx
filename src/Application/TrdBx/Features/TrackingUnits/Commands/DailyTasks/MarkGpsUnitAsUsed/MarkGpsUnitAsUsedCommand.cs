using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Application.Features.Common;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.MarkTrackingUnitAsUsed;

public class MarkTrackingUnitAsUsedCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, MarkTrackingUnitAsUsedCommand>(MemberList.None)
    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            .ForMember(dest => dest.TsDate, opt => opt.Ignore());
    //    }
    //}

}

public class MarkTrackingUnitAsUsedCommandHandler :SerialForSharedLogic , IRequestHandler<MarkTrackingUnitAsUsedCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<MarkTrackingUnitAsUsedCommandHandler> _localizer;
    //public MarkTrackingUnitAsUsedCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<MarkTrackingUnitAsUsedCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IWialonService _wialonService;

    public MarkTrackingUnitAsUsedCommandHandler(
       IApplicationDbContextFactory dbContextFactory,IWialonService wialonService)
    {
       _dbContextFactory = dbContextFactory;
       _wialonService = wialonService;
    }
    public async ValueTask<Result<int>> Handle(MarkTrackingUnitAsUsedCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await context.TrackingUnits.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.Recovered || unit.UStatus == UStatus.Damaged))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be Recovered or Damaged to procced");
        }

        var serviceNo = await GenSerialNo(context, "ServiceLog", request.TsDate);

        var serviceLog = new ServiceLog()
        {
            Description = "تعيين الحالة كمستعمل.",
            ServiceNo = serviceNo,
            ServiceTask = ServiceTask.StatusUpdate,
            CustomerId = (int)unit.CustomerId,
            SerDate = request.TsDate,
            IsDeserved =false,
            IsBilled = false,
            Amount = 0.0m,
            Subscriptions = new List<Subscription>(),
            WialonTasks = new List<WialonTask>()
        };

            serviceLog.WialonTasks.Add(new WialonTask()
            {
                TrackingUnitId = unit.Id,
                WialonAPIAction = WialonAPIAction.UpdateOnWialon,
                Description = string.Format(" تحقق من الوحدة ({0}) على منصة ويلون.", unit.SNo),
                ExcDate = request.TsDate,
                IsExecuted = false,
            });

        unit.UStatus = UStatus.Used;

        unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));

        var result = await context.SaveChangesAsync(cancellationToken);

        if (result > 0)
            return await Result<int>.SuccessAsync(unit.Id);
        else
            return await Result<int>.FailureAsync("MarkTrackingUnitAsUsed Faild!");




    }
}

