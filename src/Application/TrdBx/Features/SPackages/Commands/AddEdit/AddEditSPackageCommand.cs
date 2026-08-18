using CleanArchitecture.Blazor.Application.Features.SPackages.Caching;
// using CleanArchitecture.Blazor.Application.Features.SPackages.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Application.Features.SPackages.Commands.AddEdit;

public class AddEditSPackageCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "SProviderId")] public int SProviderId { get; set; }
    [Display(Name = "Name")] public string Name { get; set; } = string.Empty;


      public string CacheKey => SPackageCacheKey.GetAllCacheKey;
      public IEnumerable<string> Tags => SPackageCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<SPackageDto, AddEditSPackageCommand>(MemberList.None);
    //        CreateMap<AddEditSPackageCommand, SPackageDto>(MemberList.None);
    //    }
    //}
}

public class AddEditSPackageCommandHandler : IRequestHandler<AddEditSPackageCommand, Result<int>>
{
        private readonly IObjectMapper _objectMapper;
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public AddEditSPackageCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _objectMapper = objectMapper;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<Result<int>> Handle(AddEditSPackageCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        if (request.Id > 0)
        {
            var item = await context.SPackages.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"SPackage with id: [{request.Id}] not found.");
            }
           item = _objectMapper.Map(request, item);
            // raise a update domain event
            item.AddDomainEvent(new SPackageUpdatedEvent(item));
            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = _objectMapper.Map<SPackage>(request);
            // raise a create domain event
            item.AddDomainEvent(new SPackageCreatedEvent(item));
            context.SPackages.Add(item);
            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }


       
    }
}

