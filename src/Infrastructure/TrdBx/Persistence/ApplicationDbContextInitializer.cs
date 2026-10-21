using CleanArchitecture.Blazor.Application.Common.Constants;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;

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
                new ServicePrice {ServiceTask = ServiceTask.Check, Description = "Defualt system service price", Price = 10.0m },
                new ServicePrice {ServiceTask = ServiceTask.ReInstall, Description = "Defualt system service price", Price = 100.0m },
                new ServicePrice {ServiceTask = ServiceTask.Recover, Description = "Defualt system service price", Price = 25.0m },
                new ServicePrice {ServiceTask = ServiceTask.Replace, Description = "Defualt system service price", Price = 125.0m },
                new ServicePrice {ServiceTask = ServiceTask.InstallSimCard, Description = "Defualt system service price", Price = 50.0m },
                new ServicePrice {ServiceTask = ServiceTask.ReplacSimCard, Description = "Defualt system service price", Price = 50.0m },
                new ServicePrice {ServiceTask = ServiceTask.TrdbxDataUpload, Description = "Defualt system service price", Price = 0.0m },
                new ServicePrice {ServiceTask = ServiceTask.Transfer, Description = "Defualt system service price", Price = 125.0m },
            };

        await _context.ServicePrices.AddRangeAsync(servicePrice);
        await _context.SaveChangesAsync();
    }




    private async Task SeedTrdBxRolesAsync()
    {
        if (await _context.Roles.AnyAsync(r => r.Name == Roles.Installer)) return;

        var installerRoleName = Roles.Installer;
        var officeRoleName = Roles.Office;
        var accountantRoleName = Roles.Accountant;

        _logger.LogInformation("Seeding TrdBx roles...");

        var tenantId = (await _context.Tenants.FirstAsync()).Id;

        var installerRole = new ApplicationRole(installerRoleName)
        {
            Description = "Installer Group",
            //TenantId = tenantId
        };

        var officeRole = new ApplicationRole(officeRoleName)
        {
            Description = "Office Group",
            //TenantId = tenantId
        };

        var accountantRole = new ApplicationRole(accountantRoleName)
        {
            Description = "Accountant Group",
            //TenantId = tenantId
        };

        await _roleManager.CreateAsync(installerRole);
        await _roleManager.CreateAsync(officeRole);
        await _roleManager.CreateAsync(accountantRole);

    }



    private async Task SeedTrdBxUsersAsync()
    {

        if (await _context.Users.AnyAsync(r => r.UserName == Users.Installer1)) return;

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
            DisplayName = "معاد الشريف",
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

        var demoOffice1 = new ApplicationUser
        {
            UserName = Users.Office1,
            IsActive = true,
            Provider = "Local",
            TenantId = (await _context.Tenants.FirstAsync()).Id,
            DisplayName = "نجلاء محمد",
            Email = "Najlaa@gmail.com",
            EmailConfirmed = true,
            LanguageCode = "ar-LY",
            TimeZoneId = "Libya/Tripoli",
            ProfilePictureDataUrl = "https://s.gravatar.com/avatar/ea753b0b0f357a41491408307ade445e?s=80"
        };


        var demoAccountant1 = new ApplicationUser
        {
            UserName = Users.Accountant1,
            IsActive = true,
            Provider = "Local",
            TenantId = (await _context.Tenants.FirstAsync()).Id,
            DisplayName = "محمد سعد",
            Email = "Saed@gmail.com",
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

        await _userManager.CreateAsync(demoOffice1, Users.DefaultPassword);
        await _userManager.AddToRoleAsync(demoOffice1, Roles.Office);

        await _userManager.CreateAsync(demoAccountant1, Users.DefaultPassword);
        await _userManager.AddToRoleAsync(demoAccountant1, Roles.Accountant);


    }

}