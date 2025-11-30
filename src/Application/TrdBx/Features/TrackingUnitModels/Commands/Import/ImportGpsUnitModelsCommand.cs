using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Mappers;
using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.Import;

public class ImportTrackingUnitModelsCommand: ICacheInvalidatorRequest<Result<int>>
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string CacheKey => TrackingUnitModelCacheKey.GetAllCacheKey;
         public IEnumerable<string> Tags => TrackingUnitModelCacheKey.Tags;
        public ImportTrackingUnitModelsCommand(string fileName,byte[] data)
        {
           FileName = fileName;
           Data = data;
        }
    }
    public record class CreateTrackingUnitModelsTemplateCommand : IRequest<Result<byte[]>>
    {
 
    }

    public class ImportTrackingUnitModelsCommandHandler : 
                 IRequestHandler<CreateTrackingUnitModelsTemplateCommand, Result<byte[]>>,
                 IRequestHandler<ImportTrackingUnitModelsCommand, Result<int>>
    {
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportTrackingUnitModelsCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly TrackingUnitModelDto _dto = new();
    //private readonly IMapper _mapper;
    //public ImportTrackingUnitModelsCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportTrackingUnitModelsCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportTrackingUnitModelsCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly TrackingUnitModelDto _dto = new();
    public ImportTrackingUnitModelsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportTrackingUnitModelsCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }

#nullable disable warnings
    public async Task<Result<int>> Handle(ImportTrackingUnitModelsCommand request, CancellationToken cancellationToken)
        {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, TrackingUnitModelDto, object?>>
            {
                { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.WialonName)], (row, item) => item.WialonName = row[_localizer[_dto.GetMemberDescription(x=>x.WialonName)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.Name)], (row, item) => item.Name = row[_localizer[_dto.GetMemberDescription(x=>x.Name)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.WhwTypeId)], (row, item) => item.WhwTypeId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.WhwTypeId)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.PortNo1)], (row, item) => item.PortNo1 = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.PortNo1)]].ToString(), out int result) == true ? result : 0) },
                { _localizer[_dto.GetMemberDescription(x=>x.PortNo2)], (row, item) => item.PortNo2 = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.PortNo2)]].ToString(), out int result) == true ? result : 0) },
                { _localizer[_dto.GetMemberDescription(x=>x.DefualtGprs)], (row, item) => item.DefualtGprs = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.DefualtGprs)]].ToString())},
                { _localizer[_dto.GetMemberDescription(x=>x.DefualtHost)], (row, item) => item.DefualtHost = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.DefualtHost)]].ToString())},
                { _localizer[_dto.GetMemberDescription(x=>x.DefualtPrice)], (row, item) => item.DefualtPrice = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.DefualtPrice)]].ToString())},
                { _localizer[_dto.GetMemberDescription(x=>x.OldId)], (row, item) => item.OldId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.OldId)]].ToString(), out int result) == true ? result : null) }

            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.TrackingUnitModels.AnyAsync(x => x.Name == dto.Name, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<TrackingUnitModel>(dto);
                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.TrackingUnitModels.AddAsync(item, cancellationToken);
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
        public async Task<Result<byte[]>> Handle(CreateTrackingUnitModelsTemplateCommand request, CancellationToken cancellationToken)
        {
        var fields = new string[] {
            _localizer[_dto.GetMemberDescription(x=>x.WialonName)],
                  _localizer[_dto.GetMemberDescription(x=>x.Name)],
_localizer[_dto.GetMemberDescription(x=>x.WhwTypeId)],
_localizer[_dto.GetMemberDescription(x=>x.PortNo1)],
_localizer[_dto.GetMemberDescription(x=>x.PortNo2)],
_localizer[_dto.GetMemberDescription(x=>x.DefualtGprs)],
_localizer[_dto.GetMemberDescription(x=>x.DefualtHost)],
_localizer[_dto.GetMemberDescription(x=>x.DefualtPrice)],
_localizer[_dto.GetMemberDescription(x=>x.OldId)],

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);

        }
    }

