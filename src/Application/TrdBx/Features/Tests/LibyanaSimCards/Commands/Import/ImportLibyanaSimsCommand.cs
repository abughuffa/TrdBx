// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Mappers;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Commands.Import;

public class ImportLibyanaSimCardsCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }

     public IEnumerable<string> Tags => LibyanaSimCardCacheKey.Tags;
    public ImportLibyanaSimCardsCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateLibyanaSimCardsTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportLibyanaSimCardsCommandHandler :
             IRequestHandler<CreateLibyanaSimCardsTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportLibyanaSimCardsCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportLibyanaSimCardsCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly LibyanaSimCardDto _dto = new();
    //private readonly IMapper _mapper;
    //public ImportLibyanaSimCardsCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportLibyanaSimCardsCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportLibyanaSimCardsCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly LibyanaSimCardDto _dto = new();
    public ImportLibyanaSimCardsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportLibyanaSimCardsCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportLibyanaSimCardsCommand request, CancellationToken cancellationToken)
    {
    //    [Description("Active")] Active = 0,
    //[Description("One-Way Block")] OneWayBlock = 1,
    //[Description("Two-Way Block")] TwoWayBlock = 2,
    //[Description("Frozen Block")] Frozen = 3,
    //[Description("Null")] Null = 99,
    //[Description("All")] All = 999
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, LibyanaSimCardDto, object?>>
            {
{ _localizer[_dto.GetMemberDescription(x=>x.SimCardNo)], (row, item) => item.SimCardNo = row[_localizer[_dto.GetMemberDescription(x=>x.SimCardNo)]].ToString() },
//{ _localizer[_dto.GetMemberDescription(x=>x.SimCardStatus)], (row, item) => item.SimCardStatus = (SLStatus)row[_localizer[_dto.GetMemberDescription(x=>x.SimCardStatus)].ToString() = "Active" ? SLStatus.Active : SLStatus.OneWayBlock  }
//{ _localizer[_dto.GetMemberDescription(x=>x.SimCardStatus)], (row, item) => item.SimCardStatus = row[_localizer[_dto.GetMemberDescription(x=>x.SimCardStatus)]].ToString() },
{ _localizer[_dto.GetMemberDescription(x=>x.SimCardStatus)], (row, item) => item.SimCardStatus
            = row[_localizer[_dto.GetMemberDescription(x=>x.SimCardStatus)]].ToString() switch
                {
                    var x when x == "Active" => SLStatus.Active, //	*
                    var x when x == "One-Way Block"  => SLStatus.OneWayBlock, //	G	
                    var x when x == "Two-Way Block" => SLStatus.TwoWayBlock, //	H
                    var x when x == "Frozen Block" => SLStatus.Frozen, //	H+G
                    var x when x == "Inactive" => SLStatus.Inactive, //	H+G
                    _ => null // Default case when none match
                } },
{ _localizer[_dto.GetMemberDescription(x=>x.Balance)], (row, item) => item.Balance = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Balance)]].ToString())},
{ _localizer[_dto.GetMemberDescription(x=>x.BExDate)], (row, item) => item.BExDate = (DateTime.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.BExDate)]].ToString(), out DateTime result) == true ? result : null)},
{ _localizer[_dto.GetMemberDescription(x=>x.JoinDate)], (row, item) => item.JoinDate = (DateTime.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.JoinDate)]].ToString(), out DateTime result) == true ? result : null)},
{ _localizer[_dto.GetMemberDescription(x=>x.Package)], (row, item) => item.Package = row[_localizer[_dto.GetMemberDescription(x=>x.Package)]].ToString() },
{ _localizer[_dto.GetMemberDescription(x=>x.DExDate)], (row, item) => item.DExDate = (DateTime.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.DExDate)]].ToString(), out DateTime result) == true ? result : null)},
{ _localizer[_dto.GetMemberDescription(x=>x.DataOffer)], (row, item) => item.DataOffer = string.IsNullOrEmpty(row[_localizer[_dto.GetMemberDescription(x=>x.DataOffer)]].ToString()) ? null : row[_localizer[_dto.GetMemberDescription(x=>x.DataOffer)]].ToString() },
{ _localizer[_dto.GetMemberDescription(x=>x.DOExpired)], (row, item) => item.DOExpired = (DateTime.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.DOExpired)]].ToString(), out DateTime result) == true ? result : null)}
            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)

        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.LibyanaSimCards.AnyAsync(x => x.SimCardNo == dto.SimCardNo, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<LibyanaSimCard>(dto);
                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new LibyanaSimCardCreatedEvent(item));
                    await _context.LibyanaSimCards.AddAsync(item, cancellationToken);
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
    public async Task<Result<byte[]>> Handle(CreateLibyanaSimCardsTemplateCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement ImportLibyanaSimCardsCommandHandler method 
        var fields = new string[] {
                           // TODO: Define the fields that should be generate in the template, for example:
                            _localizer[_dto.GetMemberDescription(x=>x.SimCardNo)],
                            _localizer[_dto.GetMemberDescription(x=>x.SimCardStatus)],
                            _localizer[_dto.GetMemberDescription(x=>x.Balance)],
                            _localizer[_dto.GetMemberDescription(x=>x.BExDate)],
                            _localizer[_dto.GetMemberDescription(x=>x.JoinDate)],
                            _localizer[_dto.GetMemberDescription(x=>x.Package)],
                            _localizer[_dto.GetMemberDescription(x=>x.DExDate)],
                            _localizer[_dto.GetMemberDescription(x=>x.DataOffer)],
                            _localizer[_dto.GetMemberDescription(x=>x.DOExpired)],
                        };
        var result = await _excelService.CreateTemplateAsync(fields, "LibyanaSimCards" /*_localizer[_dto.GetClassDescription()]*/);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

