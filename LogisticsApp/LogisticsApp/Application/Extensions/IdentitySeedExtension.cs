using LogisticsApp.Application.Settings;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LogisticsApp.Extensions;

public static class IdentitySeedExtension
{
    public static async Task SeedIdentityAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var settings = scope.ServiceProvider.GetRequiredService<IOptions<SeedSettings>>().Value;
            
        var roles = settings.Roles;

        foreach (var role in roles)
        {
            if(!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
            
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        string email = settings.Admin.Email;
        string password = settings.Admin.Password;

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new User();
            user.UserName = email;
            user.Email = email;
            user.EmailConfirmed = true;
                
            await userManager.CreateAsync(user, password);
                
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}