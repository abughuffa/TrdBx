// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Specifications;
using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Queries.Export;

public class ExportLibyanaSimCardsQuery : LibyanaSimCardAdvancedFilter, IRequest<Result<byte[]>>
{
    public LibyanaSimCardAdvancedSpecification Specification => new(this);

}

public class ExportLibyanaSimCardsQueryHandler :
         IRequestHandler<ExportLibyanaSimCardsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportLibyanaSimCardsQueryHandler> _localizer;
    //private readonly LibyanaSimCardDto _dto = new();
    //public ExportLibyanaSimCardsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportLibyanaSimCardsQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportLibyanaSimCardsQueryHandler> _localizer;
    private readonly LibyanaSimCardDto _dto = new();
    public ExportLibyanaSimCardsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportLibyanaSimCardsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }

#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportLibyanaSimCardsQuery request, CancellationToken cancellationToken)
    {
        byte[] result;
        List<LibyanaSimCardDto> data;
        Dictionary<string, Func<LibyanaSimCardDto, object?>> mappers;

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        mappers = new Dictionary<string, Func<LibyanaSimCardDto, object?>>
                {
                       {_localizer[_dto.GetMemberDescription(x=>x.SimCardNo)],item => item.SimCardNo},
                            {_localizer[_dto.GetMemberDescription(x=>x.SimCardStatus)],item => item.SimCardStatus},
                            {_localizer[_dto.GetMemberDescription(x=>x.Balance)],item => item.Balance},
                            {_localizer[_dto.GetMemberDescription(x=>x.BExDate)],item => item.BExDate},
                            {_localizer[_dto.GetMemberDescription(x=>x.JoinDate)],item => item.JoinDate},
                            {_localizer[_dto.GetMemberDescription(x=>x.Package)],item => item.Package},
                            {_localizer[_dto.GetMemberDescription(x=>x.DExDate)],item => item.DExDate},
                            {_localizer[_dto.GetMemberDescription(x=>x.DataOffer)],item => item.DataOffer},
                            {_localizer[_dto.GetMemberDescription(x=>x.DOExpired)],item => item.DOExpired}
                };

        switch (request.ListView)
        {
            case LibyanaSimCardListView.All:
                {
                    data = await _context.LibyanaSimCards
                                               .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                               .ApplySpecification(request.Specification)
                                                .AsNoTracking()
                         .ProjectTo()
                         .ToListAsync(cancellationToken);

                    break;

                }
            case LibyanaSimCardListView.SimCardsNotExistOnTrdBx:
                {
                    var tSimCards = await _context.TrackingUnits.Select(o => o.SimCard.SimCardNo).AsNoTracking().ToListAsync(cancellationToken);

                    data = await _context.LibyanaSimCards.Where(o => !tSimCards.Contains(o.SimCardNo))
                                                           .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                           .ApplySpecification(request.Specification)
                                                           .AsNoTracking()
                         .ProjectTo()
                         .ToListAsync(cancellationToken);

                    break;

                }
            case LibyanaSimCardListView.SimCardsNotExistOnWialon:
                {
                    var wSimCards = await _context.WialonUnits.Select(o => o.SimCardNo).AsNoTracking().ToListAsync(cancellationToken);

                    data = await _context.LibyanaSimCards.Where(o => !wSimCards.Contains(o.SimCardNo))
                                                            .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                            .ApplySpecification(request.Specification)
                                                           .AsNoTracking()
                                  .ProjectTo()
                         .ToListAsync(cancellationToken);

                    break;

                }
            default:
                {
                    data = new List<LibyanaSimCardDto>();
                    break;
                }
        }

        result = await _excelService.ExportAsync(data, mappers, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);


    }
}
