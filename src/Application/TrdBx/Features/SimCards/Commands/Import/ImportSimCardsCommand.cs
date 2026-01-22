using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;
using CleanArchitecture.Blazor.Domain.Enums;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Import;

public class ImportSimCardsCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => SimCardCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => SimCardCacheKey.Tags;
    public ImportSimCardsCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateSimCardsTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportSimCardsCommandHandler :
             IRequestHandler<CreateSimCardsTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportSimCardsCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportSimCardsCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly SimCardDto _dto = new() { SimCardNo = string.Empty };
    //private readonly IMapper _mapper;
    //public ImportSimCardsCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportSimCardsCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportSimCardsCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly SimCardDto _dto = new();
    public ImportSimCardsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportSimCardsCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportSimCardsCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, SimCardDto, object?>>
            {
                { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.SPackageId)], (row, item) => item.SPackageId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.SPackageId)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.SimCardNo)], (row, item) => item.SimCardNo = row[_localizer[_dto.GetMemberDescription(x=>x.SimCardNo)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.ICCID)], (row, item) => item.ICCID = row[_localizer[_dto.GetMemberDescription(x=>x.ICCID)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.SStatus)], (row, item) => item.SStatus = (SStatus)Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x=>x.SStatus)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.ExDate)], (row, item) => item.ExDate = row[_localizer[_dto.GetMemberDescription(x=>x.ExDate)]].ToString().IsNullOrEmpty() ? null : DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.ExDate)]].ToString()))},
                //{ _localizer[_dto.GetMemberDescription(x=>x.ExDate)], (row, item) => item.ExDate = (DateOnly.TryParse((row["ExDate"] is null ? null: row["ExDate"].ToString()) , out DateOnly result) ? result : null)},
                { _localizer[_dto.GetMemberDescription(x=>x.OldId)], (row, item) => item.OldId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.OldId)]].ToString(), out int result) == true ? result : null) },
                 { _localizer[_dto.GetMemberDescription(x=>x.IsOwen)], (row, item) => item.IsOwen =Convert.ToBoolean(row[_localizer[_dto.GetMemberDescription(x=>x.IsOwen)]]) },

            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.SimCards.AnyAsync(x => x.SimCardNo == dto.SimCardNo, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<SimCard>(dto);
                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.SimCards.AddAsync(item, cancellationToken);
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
    public async Task<Result<byte[]>> Handle(CreateSimCardsTemplateCommand request, CancellationToken cancellationToken)
    {
        var fields = new string[] {
                            _localizer[_dto.GetMemberDescription(x=>x.Id)],
_localizer[_dto.GetMemberDescription(x=>x.SimCardNo)],
_localizer[_dto.GetMemberDescription(x=>x.ICCID)],
_localizer[_dto.GetMemberDescription(x=>x.SPackageId)],
_localizer[_dto.GetMemberDescription(x=>x.SStatus)],
_localizer[_dto.GetMemberDescription(x=>x.ExDate)],
_localizer[_dto.GetMemberDescription(x=>x.OldId)],
_localizer[_dto.GetMemberDescription(x=>x.IsOwen)],


                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

