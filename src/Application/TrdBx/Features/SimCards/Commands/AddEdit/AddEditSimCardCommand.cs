using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

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


    public string CacheKey => SimCardCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => SimCardCacheKey.Tags;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SimCardDto, AddEditSimCardCommand>(MemberList.None);
            CreateMap<AddEditSimCardCommand, SimCardDto>(MemberList.None);
        }
    }
}

public class AddEditSimCardCommandHandler : IRequestHandler<AddEditSimCardCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public AddEditSimCardCommandHandler(
        IMapper mapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _mapper = mapper;
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result<int>> Handle(AddEditSimCardCommand request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        if (request.Id > 0)
        {
            var item = await db.SimCards.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"SimCard with id: [{request.Id}] not found.");
            }
            item = _mapper.Map(request, item);
            // raise a update domain event
            item.AddDomainEvent(new SimCardUpdatedEvent(item));
            await db.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = _mapper.Map<SimCard>(request);
            // raise a create domain event
            item.AddDomainEvent(new SimCardCreatedEvent(item));
            db.SimCards.Add(item);
            await db.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }


    }
}

