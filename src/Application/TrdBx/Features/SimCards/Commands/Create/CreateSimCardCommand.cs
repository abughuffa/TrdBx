using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Create;

public class CreateSimCardCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "SimCardNo")]
    public string SimCardNo { get; set; } = string.Empty;
    [Display(Name = "ICCID")]
    public string? ICCID { get; set; }
    [Display(Name = "SPackageId")]
    public int SPackageId { get; set; }
    [Display(Name = "ExDate")]
    public DateOnly? ExDate { get; set; }
    [Display(Name = "IsOwen")]
    public bool IsOwen { get; set; } = true;

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

           private readonly IObjectMapper _objectMapper;
        private readonly IApplicationDbContextFactory _dbContextFactory;
        public CreateSimCardCommandHandler(
            IObjectMapper objectMapper,
            IApplicationDbContextFactory dbContextFactory)
        {
            _objectMapper = objectMapper;
            _dbContextFactory = dbContextFactory;
        }

    public async ValueTask<Result<int>> Handle(CreateSimCardCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var item = _mapper.Map<SimCard>(request);
     await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = _objectMapper.Map<SimCard>(request);

        // raise a create domain event
        item.AddDomainEvent(new SimCardCreatedEvent(item));
        context.SimCards.Add(item);
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}


