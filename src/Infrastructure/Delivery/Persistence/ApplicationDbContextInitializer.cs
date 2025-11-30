using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;
using CleanArchitecture.Blazor.Infrastructure.Constants.Role;
using CleanArchitecture.Blazor.Infrastructure.Constants.User;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence;

public partial class ApplicationDbContextInitializer
{



    public async Task SeedDeliveryAsync()
    {
        try
        {
            await SeedDeliveryRolesAsync();
            await SeedDeliveryUsersAsync();
            _context.ChangeTracker.Clear();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding Delivery database");
            throw;
        }
    }






    private async Task SeedDeliveryRolesAsync()
    {
        var traderRoleName = RoleName.Trader;
        var transporterRoleName = RoleName.Transporter;
        var driverRoleName = RoleName.Driver;

        _logger.LogInformation("Seeding Delivery roles...");


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

        await _roleManager.CreateAsync(traderRole);
        await _roleManager.CreateAsync(transporterRole);
        await _roleManager.CreateAsync(driverRole);

    }


    private async Task SeedDeliveryUsersAsync()
    {
        //if (await _userManager.Users.AnyAsync()) return;

        _logger.LogInformation("Seeding Delivery users...");

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

        await _userManager.CreateAsync(traderUser, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(traderUser, RoleName.Trader);

        await _userManager.CreateAsync(transporterUser, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(transporterUser, RoleName.Transporter);


        await _userManager.CreateAsync(driverUser, UserName.DefaultPassword);
        await _userManager.AddToRoleAsync(driverUser, RoleName.Driver);
    }

}