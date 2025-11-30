using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;
using CleanArchitecture.Blazor.Domain.Enums;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Import;

public class ImportTrackingUnitsCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    public ImportTrackingUnitsCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateTrackingUnitsTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportTrackingUnitsCommandHandler :
             IRequestHandler<CreateTrackingUnitsTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportTrackingUnitsCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportTrackingUnitsCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly TrackingUnitDto _dto = new() { SNo = string.Empty };
    //private readonly IMapper _mapper;
    //public ImportTrackingUnitsCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportTrackingUnitsCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    


    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportTrackingUnitsCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly TrackingUnitDto _dto = new();
    public ImportTrackingUnitsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportTrackingUnitsCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportTrackingUnitsCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, TrackingUnitDto, object?>>
            {
                { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.SNo)], (row, item) => item.SNo = row[_localizer[_dto.GetMemberDescription(x=>x.SNo)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.Imei)], (row, item) => item.Imei = row[_localizer[_dto.GetMemberDescription(x=>x.Imei)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.UnitName)], (row, item) => item.UnitName = row[_localizer[_dto.GetMemberDescription(x=>x.UnitName)]].ToString() != null ? row[_localizer[_dto.GetMemberDescription(x=>x.UnitName)]].ToString() : null  },
                { _localizer[_dto.GetMemberDescription(x=>x.TrackingUnitModelId)], (row, item) => item.TrackingUnitModelId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitModelId)]].ToString()) },
                //{ _localizer[_dto.GetMemberDescription(x=>x.WryDate)], (row, item) => item.WryDate = (DateOnly.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.WryDate)]].ToString(), out DateOnly result) == true ? result : null)},
                { _localizer[_dto.GetMemberDescription(x=>x.WryDate)], (row, item) => item.WryDate = row[_localizer[_dto.GetMemberDescription(x=>x.WryDate)]].ToString().IsNullOrEmpty() ? null : DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.WryDate)]].ToString()))},
                { _localizer[_dto.GetMemberDescription(x=>x.TrackedAssetId)], (row, item) => item.TrackedAssetId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.TrackedAssetId)]].ToString(), out int result) == true ? result : null) },
                { _localizer[_dto.GetMemberDescription(x=>x.SimCardId)], (row, item) => item.SimCardId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.SimCardId)]].ToString(), out int result) == true ? result : null) },
                { _localizer[_dto.GetMemberDescription(x=>x.CustomerId)], (row, item) => item.CustomerId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.CustomerId)]].ToString(), out int result) == true ? result : null) },
                { _localizer[_dto.GetMemberDescription(x=>x.UStatus)], (row, item) => item.UStatus = (UStatus)Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x=>x.UStatus)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.IsOnWialon)], (row, item) => item.IsOnWialon =Convert.ToBoolean(row[_localizer[_dto.GetMemberDescription(x=>x.IsOnWialon)]]) },
                { _localizer[_dto.GetMemberDescription(x=>x.InsMode)], (row, item) => item.InsMode = (InsMode)Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x=>x.InsMode)]].ToString()) },
                 { _localizer[_dto.GetMemberDescription(x=>x.WStatus)], (row, item) => item.WStatus = (WStatus)Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x=>x.WStatus)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.WUnitId)], (row, item) => item.WUnitId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.WUnitId)]].ToString(), out int result) == true ? result : null) },
                { _localizer[_dto.GetMemberDescription(x=>x.OldId)], (row, item) => item.OldId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.OldId)]].ToString(), out int result) == true ? result : null) }
            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.TrackingUnits.AnyAsync(x => x.SNo == dto.SNo, cancellationToken);
                if (!exists)
                {
                   
                    //var item = _mapper.Map<TrackingUnit>(dto);

                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.TrackingUnits.AddAsync(item, cancellationToken);
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
    public async Task<Result<byte[]>> Handle(CreateTrackingUnitsTemplateCommand request, CancellationToken cancellationToken)
    {
        var fields = new string[] {
                                      _localizer[_dto.GetMemberDescription(x=>x.Id)],
                   _localizer[_dto.GetMemberDescription(x=>x.SNo)],
                   _localizer[_dto.GetMemberDescription(x=>x.Imei)],
                   _localizer[_dto.GetMemberDescription(x=>x.UnitName)],
                   _localizer[_dto.GetMemberDescription(x=>x.TrackingUnitModelId)],
                   _localizer[_dto.GetMemberDescription(x=>x.WryDate)],
                   _localizer[_dto.GetMemberDescription(x=>x.TrackedAssetId)],
                   _localizer[_dto.GetMemberDescription(x=>x.SimCardId)],
                   _localizer[_dto.GetMemberDescription(x=>x.CustomerId)],
                   _localizer[_dto.GetMemberDescription(x=>x.UStatus)],
                    _localizer[_dto.GetMemberDescription(x=>x.IsOnWialon)],
                    _localizer[_dto.GetMemberDescription(x=>x.InsMode)],
                     _localizer[_dto.GetMemberDescription(x=>x.WUnitId)],
                      _localizer[_dto.GetMemberDescription(x=>x.OldId)],


                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

