using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Import;
// using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Mappers;
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
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IStringLocalizer<ImportTrackingUnitModelsCommandHandler> _localizer;
        private readonly IExcelService _excelService;
        private readonly TrackingUnitModelDto _dto = new();
        private readonly IObjectMapper _objectMapper;
        public ImportTrackingUnitModelsCommandHandler(
            IApplicationDbContextFactory dbContextFactory,
            IObjectMapper objectMapper,
            IExcelService excelService,
            IStringLocalizer<ImportTrackingUnitModelsCommandHandler> localizer)
        {
            _dbContextFactory = dbContextFactory;
            _localizer = localizer;
            _excelService = excelService;
            _objectMapper = objectMapper;
        }
        #nullable disable warnings


    public async ValueTask<Result<int>> Handle(ImportTrackingUnitModelsCommand request, CancellationToken cancellationToken)
        {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, TrackingUnitModelDto, object?>>
            {
                { _localizer[_dto.GetMemberDisplayName(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.Id)]].ToString()) },
                { _localizer[_dto.GetMemberDisplayName(x=>x.WialonName)], (row, item) => item.WialonName = row[_localizer[_dto.GetMemberDisplayName(x=>x.WialonName)]].ToString() },
                { _localizer[_dto.GetMemberDisplayName(x=>x.Name)], (row, item) => item.Name = row[_localizer[_dto.GetMemberDisplayName(x=>x.Name)]].ToString() },
                { _localizer[_dto.GetMemberDisplayName(x=>x.WhwTypeId)], (row, item) => item.WhwTypeId = int.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.WhwTypeId)]].ToString()) },
                { _localizer[_dto.GetMemberDisplayName(x=>x.PortNo1)], (row, item) => item.PortNo1 = (int.TryParse(row[_localizer[_dto.GetMemberDisplayName(x=>x.PortNo1)]].ToString(), out int result) == true ? result : 0) },
                { _localizer[_dto.GetMemberDisplayName(x=>x.PortNo2)], (row, item) => item.PortNo2 = (int.TryParse(row[_localizer[_dto.GetMemberDisplayName(x=>x.PortNo2)]].ToString(), out int result) == true ? result : 0) },
                { _localizer[_dto.GetMemberDisplayName(x=>x.DefaultGprs)], (row, item) => item.DefaultGprs = decimal.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.DefaultGprs)]].ToString())},
                { _localizer[_dto.GetMemberDisplayName(x=>x.DefaultHost)], (row, item) => item.DefaultHost = decimal.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.DefaultHost)]].ToString())},
                { _localizer[_dto.GetMemberDisplayName(x=>x.DefaultPrice)], (row, item) => item.DefaultPrice = decimal.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.DefaultPrice)]].ToString())},
                { _localizer[_dto.GetMemberDisplayName(x=>x.OldId)], (row, item) => item.OldId = (int.TryParse(row[_localizer[_dto.GetMemberDisplayName(x=>x.OldId)]].ToString(), out int result) == true ? result : null) }

            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await context.TrackingUnitModels.AnyAsync(x => x.Name == dto.Name, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<TrackingUnitModel>(dto);
                    var item = _objectMapper.Map<TrackingUnitModel>(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await context.TrackingUnitModels.AddAsync(item, cancellationToken);
                }
            }
            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result.Data.Count());
        }
        else
        {
            return await Result<int>.FailureAsync(result.Errors);
        }

        }
        public async ValueTask<Result<byte[]>> Handle(CreateTrackingUnitModelsTemplateCommand request, CancellationToken cancellationToken)
        {
        var fields = new string[] {
            _localizer[_dto.GetMemberDisplayName(x=>x.Id)],
            _localizer[_dto.GetMemberDisplayName(x=>x.WialonName)],
                  _localizer[_dto.GetMemberDisplayName(x=>x.Name)],
_localizer[_dto.GetMemberDisplayName(x=>x.WhwTypeId)],
_localizer[_dto.GetMemberDisplayName(x=>x.PortNo1)],
_localizer[_dto.GetMemberDisplayName(x=>x.PortNo2)],
_localizer[_dto.GetMemberDisplayName(x=>x.DefaultGprs)],
_localizer[_dto.GetMemberDisplayName(x=>x.DefaultHost)],
_localizer[_dto.GetMemberDisplayName(x=>x.DefaultPrice)],
_localizer[_dto.GetMemberDisplayName(x=>x.OldId)],

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);

        }
    }

