


using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Caching;
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Commands.AddEdit;

public class AddEditVehicleTypeCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = string.Empty;
    [Description("Image")]
    public byte Image { get; set; }


    public string CacheKey => VehicleTypeCacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => VehicleTypeCacheKey.Tags;
}

public class AddEditVehicleTypeCommandHandler : IRequestHandler<AddEditVehicleTypeCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public AddEditVehicleTypeCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(AddEditVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.VehicleTypes.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"VehicleType with id: [{request.Id}] not found.");
            }
            VehicleTypeMapper.ApplyChangesFrom(request,item);
			// raise a update domain event
			item.AddDomainEvent(new VehicleTypeUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(request.Id);
        }
        else
        {
            var item = VehicleTypeMapper.FromEditCommand(request);
            // raise a create domain event
			item.AddDomainEvent(new VehicleTypeCreatedEvent(item));
            _context.VehicleTypes.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
       
    }
}

