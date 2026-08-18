//// Licensed to the .NET Foundation under one or more agreements.

using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Caching;
// using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Mappers;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Commands.Update;



public class UpdateWialonUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "UnitSNo")]
    public string? UnitSNo { get; set; }
    [Display(Name = "SimCardNo")]
    public string? SimCardNo { get; set; }
    [Display(Name = "StatusOnWialon")]
    public string? StatusOnWialon { get; set; }
    [Display(Name = "Note")]
    public string? Note { get; set; }


    public string CacheKey => WialonUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => WialonUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateWialonUnitCommand, WialonUnit>(MemberList.None);
    //        CreateMap<WialonUnitDto, UpdateWialonUnitCommand>(MemberList.None);
    //    }
    //}
}

public class UpdateWialonUnitCommandHandler : IRequestHandler<UpdateWialonUnitCommand, Result<int>>
{
          private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IObjectMapper _objectMapper;
    public UpdateWialonUnitCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _objectMapper = objectMapper;
    }
    public async ValueTask<Result<int>> Handle(UpdateWialonUnitCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await context.WialonUnits.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("WialonUnit not found");


        item = _objectMapper.Map(request, item);
        // raise a update domain event
        item.AddDomainEvent(new WialonUnitUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}
