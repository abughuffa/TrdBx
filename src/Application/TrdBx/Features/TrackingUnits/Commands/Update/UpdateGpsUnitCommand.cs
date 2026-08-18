using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
// using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Update;

public class UpdateTrackingUnitCommand: ICacheInvalidatorRequest<Result<int>>
{
      [Display(Name = "Id")]
      public int Id { get; set; }
    [Display(Name = "SNo")]
    public required string SNo { get; set; }
    [Display(Name = "Imei")]
    public required string Imei { get; set; }
    [Display(Name = "TrackingUnitModelId")]
    public int TrackingUnitModelId { get; set; }


    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
      public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateTrackingUnitCommand, TrackingUnit>(MemberList.None);
    //        CreateMap<TrackingUnitDto, UpdateTrackingUnitCommand>(MemberList.None);
    //    }
    //}
}

public class UpdateTrackingUnitCommandHandler : IRequestHandler<UpdateTrackingUnitCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
          private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IObjectMapper _objectMapper;
    public UpdateTrackingUnitCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _objectMapper = objectMapper;
    }
    public async ValueTask<Result<int>> Handle(UpdateTrackingUnitCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await context.TrackingUnits.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("TrackingUnit not found");


        item = _objectMapper.Map(request, item);
        // raise a update domain event
        item.AddDomainEvent(new TrackingUnitUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}

