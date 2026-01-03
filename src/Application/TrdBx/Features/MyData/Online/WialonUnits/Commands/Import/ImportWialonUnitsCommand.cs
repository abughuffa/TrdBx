using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Mappers;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Commands.Import;

public class ImportWialonUnitsCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => WialonUnitCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => WialonUnitCacheKey.Tags;
    public ImportWialonUnitsCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateWialonUnitsTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportWialonUnitsCommandHandler :
             IRequestHandler<CreateWialonUnitsTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportWialonUnitsCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportWialonUnitsCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly WialonUnitDto _dto = new();
    //private readonly IMapper _mapper;
    //public ImportWialonUnitsCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportWialonUnitsCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportWialonUnitsCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly WialonUnitDto _dto = new();
    public ImportWialonUnitsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportWialonUnitsCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }

#nullable disable warnings
    public async Task<Result<int>> Handle(ImportWialonUnitsCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, WialonUnitDto, object?>>

    //        [Description("Null")] Null = 0,
    //[Description("Active")] Active = 1,
    //[Description("Inactive")] Inactive = 2,
    //[Description("All")] All = 999,


         {
            { _localizer[_dto.GetMemberDescription(x=>x.UnitName)], (row, item) => item.UnitName = row[_localizer[_dto.GetMemberDescription(x=>x.UnitName)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.Account)], (row, item) => item.Account = row[_localizer[_dto.GetMemberDescription(x=>x.Account)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.UnitSNo)], (row, item) => item.UnitSNo = row[_localizer[_dto.GetMemberDescription(x=>x.UnitSNo)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.SimCardNo)], (row, item) => item.SimCardNo = row[_localizer[_dto.GetMemberDescription(x=>x.SimCardNo)]].ToString().StartsWith("+218") ? row[_localizer[_dto.GetMemberDescription(x=>x.SimCardNo)]].ToString().Substring(4) : row[_localizer[_dto.GetMemberDescription(x=>x.SimCardNo)]].ToString() },
               // { _localizer[_dto.GetMemberDescription(x=>x.Deactivation)], (row, item) => item.Deactivation = string.IsNullOrEmpty(row[_localizer[_dto.GetMemberDescription(x=>x.Deactivation)]].ToString()) ? null : DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Deactivation)]].ToString())  },
               // { _localizer[_dto.GetMemberDescription(x=>x.StatusOnWialon)], (row, item) => item.StatusOnWialon = string.IsNullOrEmpty(row[_localizer[_dto.GetMemberDescription(x=>x.Deactivation)]].ToString()) ? "Active" : "Inactive"  }
                { _localizer[_dto.GetMemberDescription(x=>x.Deactivation)], (row, item) => item.Deactivation = string.IsNullOrEmpty(row[_localizer[_dto.GetMemberDescription(x=>x.Deactivation)]].ToString()) ? null : DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Deactivation)]].ToString())  },
                { _localizer[_dto.GetMemberDescription(x=>x.StatusOnWialon)], (row, item) => item.StatusOnWialon = string.IsNullOrEmpty(row[_localizer[_dto.GetMemberDescription(x=>x.Deactivation)]].ToString()) ? WStatus.Active : WStatus.Inactive  }
            }, _localizer[_dto.GetClassDescription()]);


        if (result.Succeeded && result.Data is not null)

        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.WialonUnits.AnyAsync(x => x.UnitSNo == dto.UnitSNo, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<WialonUnit>(dto);
                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new WialonUnitCreatedEvent(item));
                    await _context.WialonUnits.AddAsync(item, cancellationToken);
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
    public async Task<Result<byte[]>> Handle(CreateWialonUnitsTemplateCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement ImportWialonUnitsCommandHandler method 
        var fields = new string[] {
                   // TODO: Define the fields that should be generate in the template, for example:
                    _localizer[_dto.GetMemberDescription(x=>x.UnitName)],
                    _localizer[_dto.GetMemberDescription(x=>x.Account)],
                    _localizer[_dto.GetMemberDescription(x=>x.UnitSNo)],
                    _localizer[_dto.GetMemberDescription(x=>x.SimCardNo)],
                    _localizer[_dto.GetMemberDescription(x=>x.Deactivation)],
                    _localizer[_dto.GetMemberDescription(x=>x.StatusOnWialon)],
                    _localizer[_dto.GetMemberDescription(x=>x.Note)],
                     };
        var result = await _excelService.CreateTemplateAsync(fields, "WialonUnits");
        return await Result<byte[]>.SuccessAsync(result);
    }
}

