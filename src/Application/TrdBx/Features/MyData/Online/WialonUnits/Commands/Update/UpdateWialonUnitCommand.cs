//// Licensed to the .NET Foundation under one or more agreements.

using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Mappers;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Commands.Update;



public class UpdateWialonUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("UnitSNo")]
    public string? UnitSNo { get; set; }
    [Description("SimCardNo")]
    public string? SimCardNo { get; set; }
    [Description("StatusOnWialon")]
    public string? StatusOnWialon { get; set; }
    [Description("Note")]
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
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public UpdateWialonUnitCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public UpdateWialonUnitCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(UpdateWialonUnitCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.WialonUnits.FindAsync(request.Id, cancellationToken);
        if (item == null)
        {
            return await Result<int>.FailureAsync($"WialonUnit with id: [{request.Id}] not found.");
        }
        //item = _mapper.Map(request, item);

        Mapper.ApplyChangesFrom(request, item);
        // raise a update domain event
        item.AddDomainEvent(new WialonUnitUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}
