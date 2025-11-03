using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Create;

public class CreateSimCardCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("SimCardNo")]
    public string SimCardNo { get; set; } = string.Empty;
    [Description("ICCID")]
    public string? ICCID { get; set; }
    [Description("SPackageId")]
    public int SPackageId { get; set; }
    [Description("ExDate")]
    public DateOnly? ExDate { get; set; }

    public string CacheKey => SimCardCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => SimCardCacheKey.Tags;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateSimCardCommand, SimCard>(MemberList.None);
        }
    }
}

public class CreateSimCardCommandHandler : IRequestHandler<CreateSimCardCommand, Result<int>>
{

    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public CreateSimCardCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }


    public async Task<Result<int>> Handle(CreateSimCardCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = _mapper.Map<SimCard>(request);
        // raise a create domain event
        item.AddDomainEvent(new SimCardCreatedEvent(item));
        db.SimCards.Add(item);
        await db.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}


