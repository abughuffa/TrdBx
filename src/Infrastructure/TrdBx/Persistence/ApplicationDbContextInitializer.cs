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
        var installerRoleName = RoleName.Installer;
        var traderRoleName = RoleName.Trader;
        var transporterRoleName = RoleName.Transporter;
        var driverRoleName = RoleName.Driver;

        _logger.LogInformation("Seeding TrdBx roles...");

        var installerRole = new ApplicationRole(installerRoleName)
        {
            Description = "Installer Group",
            Created = DateTime.UtcNow,
        };

        var traderRole = new ApplicationRole(traderRoleName)
        {
            Description = "Trader Group",
            TenantId = (await _context.Tenants.FirstAsync()).Id
        };

        var transporterRole = new ApplicationRole(transporterRoleName)
        {
            Description = "Transporter Group",
            TenantId = (await _context.Tenants.FirstAsync()).Id
        };

        var driverRole = new ApplicationRole(driverRoleName)
        {
            Description = "Driver Group",
            TenantId = (await _context.Tenants.FirstAsync()).Id
        };

        await _roleManager.CreateAsync(installerRole);

        await _roleManager.CreateAsync(traderRole);
        await _roleManager.CreateAsync(transporterRole);
        await _roleManager.CreateAsync(driverRole);

    }


    private async Task SeedTrdBxUsersAsync()
    {
        //if (await _userManager.Users.AnyAsync()) return;

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

        var traderUser = new ApplicationUser
        {
            UserName = UserName.Trader,
            Provider = "Local",
            IsActive = true,
            TenantId = (await _context.Tenants.FirstAsync()).Id,
            DisplayName = UserName.Trader,
            Email = "Trader@example.com",
            EmailConfirmed = true,
            ProfilePictureDataUrl = "https://s.gravatar.com/avatar/78be68221020124c23c665ac54e07074?s=80",
            LanguageCode = "en-US",
            TimeZoneId = "Asia/Shanghai",
            TwoFactorEnabled = false
        };

        var transporterUser = new ApplicationUser
        {
            UserName = UserName.Transporter,
            Provider = "Local",
            IsActive = true,
            TenantId = (await _context.Tenants.FirstAsync()).Id,
            DisplayName = UserName.Transporter,
            Email = "Transporter@example.com",
            EmailConfirmed = true,
            ProfilePictureDataUrl = "https://s.gravatar.com/avatar/78be68221020124c23c665ac54e07074?s=80",
            LanguageCode = "en-US",
            TimeZoneId = "Asia/Shanghai",
            TwoFactorEnabled = false
        };


        var driverUser = new ApplicationUser
        {
            UserName = UserName.Driver,
            Provider = "Local",
            IsActive = true,
            TenantId = (await _context.Tenants.FirstAsync()).Id,
            DisplayName = UserName.Driver,
            Email = "Driver@example.com",
            EmailConfirmed = true,
            ProfilePictureDataUrl = "https://s.gravatar.com/avatar/78be68221020124c23c665ac54e07074?s=80",
            LanguageCode = "en-US",
            TimeZoneId = "Asia/Shanghai",
            TwoFactorEnabled = false
        };


        await _userManager.CreateAsync(demoInstaller1, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(demoInstaller1, RoleName.Installer);

        await _userManager.CreateAsync(demoInstaller2, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(demoInstaller2, RoleName.Installer);

        await _userManager.CreateAsync(demoInstaller3, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(demoInstaller3, RoleName.Installer);

        await _userManager.CreateAsync(traderUser, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(traderUser, RoleName.Trader);

        await _userManager.CreateAsync(transporterUser, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(transporterUser, RoleName.Transporter);


        await _userManager.CreateAsync(driverUser, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(driverUser, RoleName.Driver);
    }

}