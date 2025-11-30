using CleanArchitecture.Blazor.Application.Features.Drivers.Caching;
using CleanArchitecture.Blazor.Application.Features.Drivers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Drivers.Mappers;
using CleanArchitecture.Blazor.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Blazor.Application.Features.Drivers.Queries.GetMy;

public class GetMyDriversQuery : ICacheableRequest<IEnumerable<DriverDto>>
{
    public UserProfile? CurrentUser { get; set; }
    public string CacheKey => DriverCacheKey.GetMyCacheKey($"{CurrentUser.UserId}");
    public IEnumerable<string>? Tags => DriverCacheKey.Tags;
}

public class GetMyDriversQueryHandler :
     IRequestHandler<GetMyDriversQuery, IEnumerable<DriverDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetMyDriversQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<DriverDto>> Handle(GetMyDriversQuery request, CancellationToken cancellationToken)
    {
        //Expression<Func<ApplicationUser, bool>> search = (x => x.UserName == "");
            
        var data = await _userManager.Users.Where(x => (x.SuperiorId.Equals(request.CurrentUser.UserId) && 
                                                       (x.UserRoles.Any(x => x.Role.Name == "Driver"))))
                                           .Include(x => x.UserRoles).ThenInclude(ur => ur.Role)
                                           .ProjectTo()
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


