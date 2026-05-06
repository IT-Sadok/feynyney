using FluentValidation;
using LogisticsApp.Application.Validation;
using LogisticsApp.DTO;
using LogisticsApp.Extensions;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddDatabase(builder.Configuration)
            .AddJwtAuthentication(builder.Configuration)
            .AddAppIdentity()
            .AddApplication()
            .AddValidatorsFromAssemblyContaining<PackageModelValidation>()
            .AddAuthorization()
            .AddSwagger();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();
        
        app.UseHttpsRedirection();
        app.UseSwaggerIfDev();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
        app.MapPackageEndpoints();
        app.MapTransportEndpoints();
        app.MapTerminalsEndpoints();

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
        
        app.Run();
    }
}