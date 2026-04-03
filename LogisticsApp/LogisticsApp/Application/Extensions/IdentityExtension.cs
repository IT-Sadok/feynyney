using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Extensions;

public static class IdentityExtension
{
    public static IServiceCollection AddAppIdentity(this IServiceCollection services)
    {
        services
            .AddIdentityCore<User>(options => { })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}