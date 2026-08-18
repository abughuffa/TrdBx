using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Update;

public class UpdateSimCardCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "SimCardNo")]
    public required string SimCardNo { get; set; }
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
    //        CreateMap<UpdateSimCardCommand, SimCard>(MemberList.None);
    //        CreateMap<SimCardDto, UpdateSimCardCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateSimCardCommandHandler : IRequestHandler<UpdateSimCardCommand, Result<int>>
{
          private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IObjectMapper _objectMapper;
    public UpdateSimCardCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _objectMapper = objectMapper;
    }

    public async ValueTask<Result<int>> Handle(UpdateSimCardCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

  
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await context.SimCards.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("SimCard not found");


        item = _objectMapper.Map(request, item);
        // raise a update domain event
        item.AddDomainEvent(new SimCardUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}


