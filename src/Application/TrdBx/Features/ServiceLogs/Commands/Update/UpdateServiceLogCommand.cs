using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
    // using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Mappers;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Update;

public class UpdateServiceLogCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "CustomerId")]
    public int CustomerId { get; set; }
    [Display(Name = "InstallerId")]
    public int InstallerId { get; set; }
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "IsDeserved")]
    public bool IsDeserved { get; set; } = true;
    [Display(Name = "IsBilled")]
    public bool IsBilled { get; set; } = false;
    [Display(Name = "Amount")]
    public decimal Amount { get; set; } = 0.0m;


    public string CacheKey => ServiceLogCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => ServiceLogCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateServiceLogCommand, ServiceLog>(MemberList.None);
    //        CreateMap<ServiceLogDto, UpdateServiceLogCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateServiceLogCommandHandler : IRequestHandler<UpdateServiceLogCommand, Result<int>>
{
         private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IObjectMapper _objectMapper;
    public UpdateServiceLogCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _objectMapper = objectMapper;
    }
    public async ValueTask<Result<int>> Handle(UpdateServiceLogCommand request, CancellationToken cancellationToken)
    {



        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await context.ServiceLogs.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("ServiceLog not found");


        item = _objectMapper.Map(request, item);
        // raise a update domain event
        item.AddDomainEvent(new ServiceLogUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

