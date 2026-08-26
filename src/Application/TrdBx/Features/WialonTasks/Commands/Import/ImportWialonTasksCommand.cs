using CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;
// using CleanArchitecture.Blazor.Application.Features.WialonTasks.Mappers;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Commands.Import;

public class ImportWialonTasksCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => WialonTaskCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => WialonTaskCacheKey.Tags;
    public ImportWialonTasksCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateWialonTasksTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportWialonTasksCommandHandler :
             IRequestHandler<CreateWialonTasksTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportWialonTasksCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportWialonTasksCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly WialonTaskDto _dto = new() { Desc = string.Empty };
    //private readonly IMapper _mapper;
    //public ImportWialonTasksCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportWialonTasksCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}


        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IStringLocalizer<ImportWialonTasksCommandHandler> _localizer;
        private readonly IExcelService _excelService;
        private readonly WialonTaskDto _dto = new();
        private readonly IObjectMapper _objectMapper;
        public ImportWialonTasksCommandHandler(
            IApplicationDbContextFactory dbContextFactory,
            IObjectMapper objectMapper,
            IExcelService excelService,
            IStringLocalizer<ImportWialonTasksCommandHandler> localizer)
        {
            _dbContextFactory = dbContextFactory;
            _localizer = localizer;
            _excelService = excelService;
            _objectMapper = objectMapper;
        }
        #nullable disable warnings
    public async ValueTask<Result<int>> Handle(ImportWialonTasksCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, WialonTaskDto, object?>>
            {
               { _localizer[_dto.GetMemberDisplayName(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.Id)]].ToString()) },
               { _localizer[_dto.GetMemberDisplayName(x=>x.ServiceLogId)], (row, item) => item.ServiceLogId = int.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.ServiceLogId)]].ToString())},
               { _localizer[_dto.GetMemberDisplayName(x=>x.TrackingUnitId)], (row, item) => item.TrackingUnitId = int.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.TrackingUnitId)]].ToString())},
               { _localizer[_dto.GetMemberDisplayName(x=>x.Description)], (row, item) => item.Description = row[_localizer[_dto.GetMemberDisplayName(x=>x.Description)]].ToString() },
               //{ _localizer[_dto.GetMemberDisplayName(x=>x.ExcDate)], (row, item) => item.ExcDate = row[_localizer[_dto.GetMemberDisplayName(x=>x.ExcDate)]].ToString().IsNullOrEmpty() ? null : DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.ExcDate)]].ToString()))},
               { _localizer[_dto.GetMemberDisplayName(x=>x.ExcDate)], (row, item) => item.ExcDate = DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.ExcDate)]].ToString()))},
               { _localizer[_dto.GetMemberDisplayName(x=>x.WialonAPIAction)], (row, item) => item.WialonAPIAction = (WialonAPIAction)Convert.ToInt32(row[_localizer[_dto.GetMemberDisplayName(x=>x.WialonAPIAction)]].ToString()) },
               { _localizer[_dto.GetMemberDisplayName(x=>x.IsExecuted)], (row, item) => item.IsExecuted = bool.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.IsExecuted)]].ToString())},

            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await context.WialonTasks.AnyAsync(x => x.Id == dto.Id, cancellationToken);
                if (!exists)
                {
                    ////var item = _mapper.Map<WialonTask>(dto);

                    var item = _objectMapper.Map<WialonTask>(dto);

                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await context.WialonTasks.AddAsync(item, cancellationToken);
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
    public async ValueTask<Result<byte[]>> Handle(CreateWialonTasksTemplateCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement ImportWialonTasksCommandHandler method 
        var fields = new string[] {
                   // TODO: Define the fields that should be generate in the template, for example:
                    _localizer[_dto.GetMemberDisplayName(x=>x.Id)],
                    _localizer[_dto.GetMemberDisplayName(x=>x.ServiceLogId)],
                    _localizer[_dto.GetMemberDisplayName(x=>x.TrackingUnitId)],
                    _localizer[_dto.GetMemberDisplayName(x=>x.Description)],
                    _localizer[_dto.GetMemberDisplayName(x=>x.WialonAPIAction)],
                    _localizer[_dto.GetMemberDisplayName(x=>x.ExcDate)],
                    _localizer[_dto.GetMemberDisplayName(x=>x.IsExecuted)]
                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}
