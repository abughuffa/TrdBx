using CleanArchitecture.Blazor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Blazor.Application.Features.Identity.DTOs;
using CleanArchitecture.Blazor.Application.Features.Installers.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Installers.Queries.GetAvaliable;

public class GetAvaliableInstallersQuery : ICacheableRequest<IEnumerable<ApplicationUserDto>>
{
    public string CacheKey => InstallerCacheKey.GetAvaliableCacheKey;
    public IEnumerable<string> Tags => InstallerCacheKey.Tags;
}
public class GetAvaliableInstallersQueryHandler :
     IRequestHandler<GetAvaliableInstallersQuery, IEnumerable<ApplicationUserDto>>
{

    private readonly IIdentityService _userService;

    public GetAvaliableInstallersQueryHandler(
      IIdentityService userService)
    {
        _userService = userService;
    }


    public async Task<IEnumerable<ApplicationUserDto>> Handle(GetAvaliableInstallersQuery request, CancellationToken cancellationToken)
    {
        var data = await _userService.GetUsers(null, cancellationToken);

         var result = data.Where(x => x.IsActive && x.AssignedRoles.Contains("Installer"));

        return result;
    }
}
