
using CleanArchitecture.Blazor.Application.Common.Constants;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence;

public partial class ApplicationDbContextInitializer
{

    private async Task SeedServicePrices()
    {
        if (await _context.ServicePrices.AnyAsync()) return;

        _logger.LogInformation("Seeding Service Price...");
        var servicePrice = new[]
            {
                new ServicePrice {ServiceTask = ServiceTask.Check, Desc = "Defualt system service price", Price = 10.0m },
                new ServicePrice {ServiceTask = ServiceTask.ReInstall, Desc = "Defualt system service price", Price = 100.0m },
                new ServicePrice {ServiceTask = ServiceTask.Recover, Desc = "Defualt system service price", Price = 25.0m },
                new ServicePrice {ServiceTask = ServiceTask.Replace, Desc = "Defualt system service price", Price = 125.0m },
                new ServicePrice {ServiceTask = ServiceTask.InstallSimCard, Desc = "Defualt system service price", Price = 50.0m },
                new ServicePrice {ServiceTask = ServiceTask.ReplacSimCard, Desc = "Defualt system service price", Price = 50.0m },
                new ServicePrice {ServiceTask = ServiceTask.TrdbxDataUpload, Desc = "Defualt system service price", Price = 0.0m },
                
            };

        await _context.ServicePrices.AddRangeAsync(servicePrice);
        await _context.SaveChangesAsync();
    }




    private async Task SeedTrdBxRolesAsync()
    {
        var installerRoleName = Roles.Installer;

        _logger.LogInformation("Seeding TrdBx roles...");

        var installerRole = new ApplicationRole(installerRoleName)
        {
            Description = "Installer Group",
            CreatedAt = DateTime.UtcNow,
        };

        await _roleManager.CreateAsync(installerRole);

    }


    private async Task SeedTrdBxUsersAsync()
    {
        //if (await _userManager.Users.AnyAsync()) return;

        _logger.LogInformation("Seeding TrdBx users...");

        var demoInstaller1 = new ApplicationUser
        {
            UserName = Users.Installer1,
            IsActive = true,
            Provider = "Local",
            TenantId = (await _context.Tenants.FirstAsync()).Id,
            DisplayName = "رضوان خالد العامري",
            Email = "redwan@gmail.com",
            EmailConfirmed = true,
            LanguageCode = "ar-LY",
            TimeZoneId = "Libya/Tripoli",
            ProfilePictureDataUrl = "https://s.gravatar.com/avatar/ea753b0b0f357a41491408307ade445e?s=80"
        };
    
        var demoInstaller2 = new ApplicationUser
        {
            UserName = Users.Installer2,
            IsActive = true,
            Provider = "Local",
            TenantId = (await _context.Tenants.FirstAsync()).Id,
            DisplayName = "محمد اعمار ابوربعية",
            Email = "Mohammed@gmail.com",
            EmailConfirmed = true,
            LanguageCode = "ar-LY",
            TimeZoneId = "Libya/Tripoli",
            ProfilePictureDataUrl = "https://s.gravatar.com/avatar/ea753b0b0f357a41491408307ade445e?s=80"
        };

        var demoInstaller3 = new ApplicationUser
        {
            UserName = Users.Installer3,
            IsActive = true,
            Provider = "Local",
            TenantId = (await _context.Tenants.FirstAsync()).Id,
            DisplayName = "خالد الهوني",
            Email = "Khaled@gmail.com",
            EmailConfirmed = true,
            LanguageCode = "ar-LY",
            TimeZoneId = "Libya/Tripoli",
            ProfilePictureDataUrl = "https://s.gravatar.com/avatar/ea753b0b0f357a41491408307ade445e?s=80"
        };



        await _userManager.CreateAsync(demoInstaller1, Users.DefaultPassword);
        await _userManager.AddToRoleAsync(demoInstaller1, Roles.Installer);

        await _userManager.CreateAsync(demoInstaller2, Users.DefaultPassword);
        await _userManager.AddToRoleAsync(demoInstaller2, Roles.Installer);

        await _userManager.CreateAsync(demoInstaller3, Users.DefaultPassword);
        await _userManager.AddToRoleAsync(demoInstaller3, Roles.Installer);
    }

}