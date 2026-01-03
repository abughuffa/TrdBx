using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.AddEdit;

public class AddEditSimCardCommand : ICacheInvalidatorRequest<Result<int>>
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
    //        CreateMap<SimCardDto, AddEditSimCardCommand>(MemberList.None);
    //        CreateMap<AddEditSimCardCommand, SimCardDto>(MemberList.None);
    //    }
    //}
}

public class AddEditSimCardCommandHandler : IRequestHandler<AddEditSimCardCommand, Result<int>>
{
    //private readonly IMapper _mapper;
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public AddEditSimCardCommandHandler(
    //    IMapper mapper,
    //    IApplicationDbContextFactory dbContextFactory)
    //{
    //    _mapper = mapper;
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public AddEditSimCardCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(AddEditSimCardCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        if (request.Id > 0)
        {
            var item = await _context.SimCards.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"SimCard with id: [{request.Id}] not found.");
            }

            //item = _mapper.Map(request, item);
            Mapper.ApplyChangesFrom(request, item);
            // raise a update domain event
            item.AddDomainEvent(new SimCardUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            //var item = _mapper.Map<SimCard>(request);
            var item = Mapper.FromEditCommand(request);
            // raise a create domain event
            item.AddDomainEvent(new SimCardCreatedEvent(item));
            _context.SimCards.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }


    }
}

