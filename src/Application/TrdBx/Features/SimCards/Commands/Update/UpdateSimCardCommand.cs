using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Update;

public class UpdateSimCardCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("SimCardNo")]
    public required string SimCardNo { get; set; }
    [Description("ICCID")]
    public string? ICCID { get; set; }
    [Description("SPackageId")]
    public int SPackageId { get; set; }
    [Description("ExDate")]
    public DateOnly? ExDate { get; set; }

    [Description("IsOwen")]
    public bool IsOwen { get; set; } = true;

    public string CacheKey => SimCardCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => SimCardCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateSimCardCommand, SimCard>(MemberList.None);
    //        CreateMap<SimCardDto, UpdateSimCardCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateSimCardCommandHandler : IRequestHandler<UpdateSimCardCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public UpdateSimCardCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public UpdateSimCardCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(UpdateSimCardCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.SimCards.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("SimCard not found");
        //_mapper.Map(request, item);
        Mapper.ApplyChangesFrom(request, item);
        // raise a update domain event
        item.AddDomainEvent(new SimCardUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}


