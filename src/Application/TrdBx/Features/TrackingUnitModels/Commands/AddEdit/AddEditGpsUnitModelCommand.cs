using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.AddEdit;

public class AddEditTrackingUnitModelCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("WialonName")] public string WialonName { get; set; } = string.Empty;
    [Description("Name")] public string Name { get; set; } = string.Empty;
    [Description("WhwTypeId")] public int WhwTypeId { get; set; }
    [Description("PortNo1")] public int PortNo1 { get; set; } = 0;
    [Description("PortNo2")] public int PortNo2 { get; set; } = 0;
    [Description("DefualtHost")] public decimal DefualtHost { get; set; } = 0.0m;
    [Description("DefualtGprs")] public decimal DefualtGprs { get; set; } = 0.0m;
    [Description("DefualtPrice")] public decimal DefualtPrice { get; set; } = 0.0m;



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
    //private readonly IMapper _mapper;
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public AddEditTrackingUnitModelCommandHandler(
    //    IMapper mapper,
    //    IApplicationDbContextFactory dbContextFactory)
    //{
    //    _mapper = mapper;
    //    _dbContextFactory = dbContextFactory;
    //}


    private readonly IApplicationDbContext _context;
    public AddEditTrackingUnitModelCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(AddEditTrackingUnitModelCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        if (request.Id > 0)
        {
            var item = await _context.TrackingUnitModels.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"TrackingUnitModel with id: [{request.Id}] not found.");
            }
            //item = _mapper.Map(request, item);
            Mapper.ApplyChangesFrom(request, item);
            // raise a update domain event
            item.AddDomainEvent(new TrackingUnitModelUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            //var item = _mapper.Map<TrackingUnitModel>(request);
            var item = Mapper.FromEditCommand(request);
            // raise a create domain event
            item.AddDomainEvent(new TrackingUnitModelCreatedEvent(item));
            _context.TrackingUnitModels.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }

       
    }
}

