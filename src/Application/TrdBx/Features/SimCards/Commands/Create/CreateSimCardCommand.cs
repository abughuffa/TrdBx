using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;

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
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<CreateSimCardCommand, SimCard>(MemberList.None);
    //    }
    //}
}

public class CreateSimCardCommandHandler : IRequestHandler<CreateSimCardCommand, Result<int>>
{

    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public CreateSimCardCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public CreateSimCardCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateSimCardCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var item = _mapper.Map<SimCard>(request);

        var item = Mapper.FromCreateCommand(request);
        // raise a create domain event
        item.AddDomainEvent(new SimCardCreatedEvent(item));
        _context.SimCards.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}


