using CleanArchitecture.Blazor.Application.Features.SPackages.Caching;
using CleanArchitecture.Blazor.Application.Features.SPackages.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SPackages.Commands.AddEdit;

public class AddEditSPackageCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("Name")] public string Name { get; set; } = string.Empty;


      public string CacheKey => SPackageCacheKey.GetAllCacheKey;
      public IEnumerable<string> Tags => SPackageCacheKey.Tags;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SPackageDto, AddEditSPackageCommand>(MemberList.None);
            CreateMap<AddEditSPackageCommand, SPackageDto>(MemberList.None);
        }
    }
}

public class AddEditSPackageCommandHandler : IRequestHandler<AddEditSPackageCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public AddEditSPackageCommandHandler(
        IMapper mapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _mapper = mapper;
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result<int>> Handle(AddEditSPackageCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        if (request.Id > 0)
        {
            var item = await db.SPackages.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"SPackage with id: [{request.Id}] not found.");
            }
            item = _mapper.Map(request, item);
            // raise a update domain event
            item.AddDomainEvent(new SPackageUpdatedEvent(item));
            await db.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = _mapper.Map<SPackage>(request);
            // raise a create domain event
            item.AddDomainEvent(new SPackageCreatedEvent(item));
            db.SPackages.Add(item);
            await db.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }


       
    }
}

