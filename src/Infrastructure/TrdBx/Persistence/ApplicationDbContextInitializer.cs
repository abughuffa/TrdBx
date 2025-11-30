using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;
using CleanArchitecture.Blazor.Infrastructure.Constants.Role;
using CleanArchitecture.Blazor.Infrastructure.Constants.User;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence;

public partial class ApplicationDbContextInitializer
{



    public async Task SeedTrdBxAsync()
    {
        try
        {
            await SeedTrdBxRolesAsync();
            await SeedTrdBxUsersAsync();
            await SeedServicePrices();

            _context.ChangeTracker.Clear();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

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
        if (await _context.Roles.AnyAsync(r => r.Name == RoleName.Installer)) return;

        var installerRoleName = RoleName.Installer;
        _logger.LogInformation("Seeding TrdBx roles...");

        var tenantId = (await _context.Tenants.FirstAsync()).Id;

        var installerRole = new ApplicationRole(installerRoleName)
        {
            Description = "Installer Group",
            TenantId = tenantId
        };
        await _roleManager.CreateAsync(installerRole);
    }


    private async Task SeedTrdBxUsersAsync()
    {

        if (await _context.Users.AnyAsync(r => r.UserName == UserName.Installer1)) return;

        _logger.LogInformation("Seeding TrdBx users...");

        var demoInstaller1 = new ApplicationUser
        {
            UserName = UserName.Installer1,
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
            UserName = UserName.Installer2,
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
            UserName = UserName.Installer3,
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

        await _userManager.CreateAsync(demoInstaller1, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(demoInstaller1, RoleName.Installer);

        await _userManager.CreateAsync(demoInstaller2, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(demoInstaller2, RoleName.Installer);

        await _userManager.CreateAsync(demoInstaller3, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(demoInstaller3, RoleName.Installer);


    }

}