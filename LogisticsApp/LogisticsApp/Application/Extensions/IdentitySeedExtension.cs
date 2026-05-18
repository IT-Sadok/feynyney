using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Extensions;

public static class IdentitySeedExtension
{
    public static async Task SeedIdentityAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            var roles = new[] {"Admin", "User"};

            foreach (var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string email = "admin@gmail.com";
            string password = "Admin1!";

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
}