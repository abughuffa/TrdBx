using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonUnits.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Import;

public class ImportTrackedAssetsCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => TrackedAssetCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackedAssetCacheKey.Tags;
    public ImportTrackedAssetsCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}

public record class CreateTrackedAssetsTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportTrackedAssetsCommandHandler :
             IRequestHandler<CreateTrackedAssetsTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportTrackedAssetsCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportTrackedAssetsCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly TrackedAssetDto _dto = new();
    //private readonly IMapper _mapper;
    //public ImportTrackedAssetsCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportTrackedAssetsCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportTrackedAssetsCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly TrackedAssetDto _dto = new();
    public ImportTrackedAssetsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportTrackedAssetsCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }

#nullable disable warnings
    public async Task<Result<int>> Handle(ImportTrackedAssetsCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, TrackedAssetDto, object?>>
            {
               { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.TrackedAssetNo)], (row, item) => item.TrackedAssetNo = row[_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetNo)]].ToString() },
               { _localizer[_dto.GetMemberDescription(x=>x.TrackedAssetCode)], (row, item) => item.TrackedAssetCode = row[_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetCode)]].ToString() },
               { _localizer[_dto.GetMemberDescription(x=>x.VinSerNo)], (row, item) => item.VinSerNo = row[_localizer[_dto.GetMemberDescription(x=>x.VinSerNo)]].ToString() },
               { _localizer[_dto.GetMemberDescription(x=>x.PlateNo)], (row, item) => item.PlateNo = row[_localizer[_dto.GetMemberDescription(x=>x.PlateNo)]].ToString() },
               { _localizer[_dto.GetMemberDescription(x=>x.TrackedAssetDesc)], (row, item) => item.TrackedAssetDesc = row[_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetDesc)]].ToString() },
               { _localizer[_dto.GetMemberDescription(x=>x.IsAvaliable)], (row, item) => item.IsAvaliable =Convert.ToBoolean(row[_localizer[_dto.GetMemberDescription(x=>x.IsAvaliable)]]) },
                { _localizer[_dto.GetMemberDescription(x=>x.OldId)], (row, item) => item.OldId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.OldId)]].ToString(), out int result) == true ? result : null) },
                 { _localizer[_dto.GetMemberDescription(x=>x.OldVehicleNo)], (row, item) => item.OldVehicleNo = row[_localizer[_dto.GetMemberDescription(x=>x.OldVehicleNo)]].ToString() },

            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.TrackedAssets.AnyAsync(x => x.TrackedAssetNo == dto.TrackedAssetNo, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<TrackedAsset>(dto);
                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.TrackedAssets.AddAsync(item, cancellationToken);
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result.Data.Count());
        }
        else
        {
            return await Result<int>.FailureAsync(result.Errors);
        }
    }
    public async Task<Result<byte[]>> Handle(CreateTrackedAssetsTemplateCommand request, CancellationToken cancellationToken)
    {
        var fields = new string[] {
                                      _localizer[_dto.GetMemberDescription(x=>x.TrackedAssetNo)],
_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetCode)],
_localizer[_dto.GetMemberDescription(x=>x.VinSerNo)],
_localizer[_dto.GetMemberDescription(x=>x.PlateNo)],
_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetDesc)],
_localizer[_dto.GetMemberDescription(x=>x.IsAvaliable)],
_localizer[_dto.GetMemberDescription(x=>x.OldId)],
_localizer[_dto.GetMemberDescription(x=>x.OldVehicleNo)],

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}


