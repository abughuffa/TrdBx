
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Blazor.Application.TrdBx.Services;
namespace CleanArchitecture.Blazor.Application;

/// <summary>
/// Dependency injection configuration for infrastructure services
/// </summary>
public static partial class DependencyInjection
{

    /// <summary>
    /// Adds TrdBx DropDown services to the DI container
    /// </summary>
    private static IServiceCollection AddDropDownServices(this IServiceCollection services)
    {
        services.AddScoped<IDropdownDataService, DropdownDataService>();
        return services;
    }
}
 