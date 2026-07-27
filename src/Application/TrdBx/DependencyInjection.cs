/* using CleanArchitecture.Blazor.Application.Common.PublishStrategies;
using CleanArchitecture.Blazor.Application.Pipeline;
using CleanArchitecture.Blazor.Application.Pipeline.PreProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using CleanArchitecture.Blazor.Application.Services;
namespace CleanArchitecture.Blazor.Infrastructure;

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
 */