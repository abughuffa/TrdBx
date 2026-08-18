using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Caching;
// using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.AddEdit;

public class AddEditTrackingUnitModelCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "WialonName")] public string WialonName { get; set; } = string.Empty;
    [Display(Name = "Name")] public string Name { get; set; } = string.Empty;
    [Display(Name = "WhwTypeId")] public int WhwTypeId { get; set; }
    [Display(Name = "PortNo1")] public int PortNo1 { get; set; } = 0;
    [Display(Name = "PortNo2")] public int PortNo2 { get; set; } = 0;
    [Display(Name = "DefualtHost")] public decimal DefualtHost { get; set; } = 0.0m;
    [Display(Name = "DefualtGprs")] public decimal DefualtGprs { get; set; } = 0.0m;
    [Display(Name = "DefualtPrice")] public decimal DefualtPrice { get; set; } = 0.0m;



    public string CacheKey => TrackingUnitModelCacheKey.GetAllCacheKey;
      public IEnumerable<string> Tags => TrackingUnitModelCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    //public Mapping()
    //    //{
    //    //    CreateMap<TrackingUnitModelDto, AddEditTrackingUnitModelCommand>(MemberList.None);
    //    //    CreateMap<AddEditTrackingUnitModelCommand, TrackingUnitModel>(MemberList.None);
    //    //}
    //}
}

public class AddEditTrackingUnitModelCommandHandler : IRequestHandler<AddEditTrackingUnitModelCommand, Result<int>>
{
      private readonly IObjectMapper _objectMapper;
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public AddEditTrackingUnitModelCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _objectMapper = objectMapper;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<Result<int>> Handle(AddEditTrackingUnitModelCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        if (request.Id > 0)
        {
            var item = await context.TrackingUnitModels.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"TrackingUnitModel with id: [{request.Id}] not found.");
            }
            item = _objectMapper.Map(request, item);
            // raise a update domain event
            item.AddDomainEvent(new TrackingUnitModelUpdatedEvent(item));
            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = _objectMapper.Map<TrackingUnitModel>(request);
            // raise a create domain event
            item.AddDomainEvent(new TrackingUnitModelCreatedEvent(item));
            context.TrackingUnitModels.Add(item);
            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }

       
    }
}

