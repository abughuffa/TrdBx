using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
// using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Create;

public class CreateTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "SNo")]
    public  string SNo { get; set; } = string.Empty;
    [Display(Name = "Imei")]
    public  string Imei { get; set; } = string.Empty;
    [Display(Name = "TrackingUnitModelId")]
    public int TrackingUnitModelId { get; set; }

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<CreateTrackingUnitCommand, TrackingUnit>(MemberList.None);
    //    }
    //}
}

public class CreateTrackingUnitCommandHandler : IRequestHandler<CreateTrackingUnitCommand, Result<int>>
{

             private readonly IObjectMapper _objectMapper;
        private readonly IApplicationDbContextFactory _dbContextFactory;
        public CreateTrackingUnitCommandHandler(
            IObjectMapper objectMapper,
            IApplicationDbContextFactory dbContextFactory)
        {
            _objectMapper = objectMapper;
            _dbContextFactory = dbContextFactory;
        }


    public async ValueTask<Result<int>> Handle(CreateTrackingUnitCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var item = _mapper.Map<TrackingUnit>(request);

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = _objectMapper.Map<TrackingUnit>(request);


        // raise a create domain event
        item.AddDomainEvent(new TrackingUnitCreatedEvent(item));
        context.TrackingUnits.Add(item);
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}

