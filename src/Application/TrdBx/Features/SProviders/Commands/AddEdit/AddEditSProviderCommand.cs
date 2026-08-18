using CleanArchitecture.Blazor.Application.Features.SProviders.Caching;
// using CleanArchitecture.Blazor.Application.Features.SProviders.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.SProviders.Commands.AddEdit;

public class AddEditSProviderCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "Name")] public string Name { get; set; } = string.Empty;


      public string CacheKey => SProviderCacheKey.GetAllCacheKey;
      public IEnumerable<string> Tags => SProviderCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<SProviderDto, AddEditSProviderCommand>(MemberList.None);
    //        CreateMap<AddEditSProviderCommand, SProviderDto>(MemberList.None);
    //    }
    //}
}

public class AddEditSProviderCommandHandler : IRequestHandler<AddEditSProviderCommand, Result<int>>
{
       private readonly IObjectMapper _objectMapper;
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public AddEditSProviderCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _objectMapper = objectMapper;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<Result<int>> Handle(AddEditSProviderCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        if (request.Id > 0)
        {
            var item = await context.SProviders.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"SProvider with id: [{request.Id}] not found.");
            }
            item = _objectMapper.Map(request, item);
            // raise a update domain event
            item.AddDomainEvent(new SProviderUpdatedEvent(item));
            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = _objectMapper.Map<SProvider>(request);
            // raise a create domain event
            item.AddDomainEvent(new SProviderCreatedEvent(item));
            context.SProviders.Add(item);
            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }


       
    }
}

