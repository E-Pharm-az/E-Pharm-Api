using EPharm.Domain.Models.Identity;
using EPharm.Infrastructure.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace EPharmApi.Services;

public class DbSeeder(
    UserManager<AppIdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IConfiguration configuration)
{
    public async Task SeedSuperAdminAsync()
    {
        try
        {
            if (!await roleManager.RoleExistsAsync(IdentityData.SuperAdmin))
            {
                await roleManager.CreateAsync(new IdentityRole(IdentityData.SuperAdmin));
                await roleManager.CreateAsync(new IdentityRole(IdentityData.Admin));
            }

            var superAdmin = await userManager.FindByEmailAsync(configuration["SuperAdmin:Email"]!);

            if (superAdmin is null)
            {
                superAdmin = new AppIdentityUser
                {
                    UserName = configuration["SuperAdmin:UserName"],
                    Email = configuration["SuperAdmin:Email"],
                    FirstName = configuration["SuperAdmin:FirstName"],
                    LastName = configuration["SuperAdmin:LastName"],
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(superAdmin, configuration["SuperAdmin:Password"]!);
                   
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, IdentityData.SuperAdmin);
                    await userManager.AddToRoleAsync(superAdmin, IdentityData.Admin);
                    
                    Log.Information("SuperAdmin user created successfully.");
                }
                else
                {
                    Log.Warning("Failed to create SuperAdmin user: {@error}", result.Errors);
                }
            }
            else
            {
                Log.Information("SuperAdmin user already exists.");
            }
        }
        catch (Exception ex)
        {
            Log.Warning("Failed to seed SuperAdmin: {error}", ex.Message);
        }
    }
}
