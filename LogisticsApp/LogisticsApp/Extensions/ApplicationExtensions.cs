using LogisticsApp.Application.Auth;
using LogisticsApp.Application.Packages;
using LogisticsApp.Application.Users;

namespace LogisticsApp.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IPackageService,  PackageService>();
        services.AddScoped<ITrackingNumberGenerator, TrackingNumberGenerator>();
        
        return services;
    }
}