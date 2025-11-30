// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.DbMatchings.Caching;
using CleanArchitecture.Blazor.Application.Features.DbMatchings.DTOs;
using CleanArchitecture.Blazor.Application.Features.DbMatchings.Specifications;
using CleanArchitecture.Blazor.Application.Features.DbMatchings.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.DbMatchings.Queries.Pagination;


public class DbMatchingsWithPaginationQuery : DbMatchingAdvancedFilter, ICacheableRequest<PaginatedData<DbMatchingDto>>
{
    public IEnumerable<string>? Tags => DbMatchingCacheKey.Tags;
    public DbMatchingAdvancedSpecification Specification => new(this);
    public string CacheKey => DbMatchingCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword},StatusOnWialon:{StatusOnWialon},StatusOnTrdBx:{StatusOnTrdBx}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }

}

public class DbMatchingsWithPaginationQueryHandler :
         IRequestHandler<DbMatchingsWithPaginationQuery, PaginatedData<DbMatchingDto>?>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public DbMatchingsWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public DbMatchingsWithPaginationQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedData<DbMatchingDto>?> Handle(DbMatchingsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        PaginatedData<DbMatchingDto>  data;

        switch (request.ListView)
        {
            case DbMatchingListView.MatchedBySimCardOnly:
                {

                    data = await (from w in _context.WialonUnits
                                  join t in _context.TrackingUnits on w.SimCardNo equals t.SimCard.SimCardNo
                                  where w.UnitSNo != t.SNo && w.SimCardNo != null && t.SimCard != null
                                  select new DbMatching
                                  {
                                      Account = w.Account,
                                      Client = t.Customer.Parent != null ? t.Customer.Parent.Name : null,
                                      Customer = t.Customer.Name,
                                      WUnitSNo = w.UnitSNo,
                                      TUnitSNo = t.SNo,
                                      WSimCardNo = w.SimCardNo,
                                      TSimCardNo = t.SimCard.SimCardNo,
                                      StatusOnWialon = w.StatusOnWialon,
                                      StatusOnTrdBx = t.UStatus,
                                      WNote = w.Note
                                  }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                  .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);
                    break;
                }
            case DbMatchingListView.MatchedByUnitOnly:
                {
                    data = await (from w in _context.WialonUnits
                    join t in _context.TrackingUnits on w.UnitSNo equals t.SNo
                    where w.SimCardNo != t.SimCard.SimCardNo && w.UnitSNo != null && t.SimCard != null
                 select new DbMatching
                 {
                     Account = w.Account,
                     Client = t.Customer.Parent != null ? t.Customer.Parent.Name : null,
                     Customer = t.Customer.Name,
                     WUnitSNo = w.UnitSNo,
                     TUnitSNo = t.SNo,
                     WSimCardNo = w.SimCardNo,
                     TSimCardNo = t.SimCard.SimCardNo,
                     StatusOnWialon = w.StatusOnWialon,
                     StatusOnTrdBx = t.UStatus,
                     WNote = w.Note
                 }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                   .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);
                    break;
                }
            case DbMatchingListView.MatchedByUnitAndSimCard:
                {

        data = await (from w in _context.WialonUnits
                      join t in _context.TrackingUnits on w.UnitSNo equals t.SNo
                      where w.SimCardNo == t.SimCard.SimCardNo && w.UnitSNo != null && t.SimCard != null
                      select new DbMatching
                      {
                          Account = w.Account,
                          Client = t.Customer.Parent != null ? t.Customer.Parent.Name : null,
                          Customer = t.Customer.Name,
                          WUnitSNo = w.UnitSNo,
                          TUnitSNo = t.SNo,
                          WSimCardNo = w.SimCardNo,
                          TSimCardNo = t.SimCard.SimCardNo,
                          StatusOnWialon = w.StatusOnWialon,
                          StatusOnTrdBx = t.UStatus,
                          WNote = w.Note
                      }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                         .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);
                    break;

                }

        
            default:
                {
                    return null;
                }
        }


        return data;
    }
}





