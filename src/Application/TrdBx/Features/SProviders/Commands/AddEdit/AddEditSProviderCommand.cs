using CleanArchitecture.Blazor.Application.Features.SProviders.Caching;
using CleanArchitecture.Blazor.Application.Features.SProviders.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SProviders.Commands.AddEdit;

public class AddEditSProviderCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("Name")] public string Name { get; set; } = string.Empty;


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
    //private readonly IMapper _mapper;
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public AddEditSProviderCommandHandler(
    //    IMapper mapper,
    //    IApplicationDbContextFactory dbContextFactory)
    //{
    //    _mapper = mapper;
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public AddEditSProviderCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(AddEditSProviderCommand request, CancellationToken cancellationToken)
    {

        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        if (request.Id > 0)
        {
            var item = await _context.SProviders.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"SProvider with id: [{request.Id}] not found.");
            }
            //item = _mapper.Map(request, item);
            Mapper.ApplyChangesFrom(request, item);
            // raise a update domain event
            item.AddDomainEvent(new SProviderUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            //var item = _mapper.Map<SProvider>(request);
            var item = Mapper.FromEditCommand(request);
            // raise a create domain event
            item.AddDomainEvent(new SProviderCreatedEvent(item));
            _context.SProviders.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }


       
    }
}

