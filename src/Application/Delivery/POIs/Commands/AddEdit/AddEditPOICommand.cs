


using CleanArchitecture.Blazor.Application.Features.POIs.Caching;
using CleanArchitecture.Blazor.Application.Features.POIs.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.POIs.Commands.AddEdit;

public class AddEditPOICommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = string.Empty;
    [Description("Latitude")]
    public double Latitude { get; set; } = 0.0;
    [Description("Longitude")]
    public double Longitude { get; set; } = 0.0;


    public string CacheKey => POICacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => POICacheKey.Tags;
}

public class AddEditPOICommandHandler : IRequestHandler<AddEditPOICommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public AddEditPOICommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(AddEditPOICommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.POIs.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"POI with id: [{request.Id}] not found.");
            }
            POIMapper.ApplyChangesFrom(request,item);
			// raise a update domain event
			item.AddDomainEvent(new POIUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = POIMapper.FromEditCommand(request);
            // raise a create domain event
			item.AddDomainEvent(new POICreatedEvent(item));
            _context.POIs.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
       
    }
}

