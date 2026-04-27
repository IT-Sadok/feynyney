using LogisticsApp.Application.Auth;
using LogisticsApp.Application.Packages;
using LogisticsApp.Application.Repositories;
using LogisticsApp.Application.Terminals;
using LogisticsApp.Application.Transports;
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
        services.AddScoped<ITransportService,  TransportService>();
        services.AddScoped<ITerminalService,  TerminalService>();
        services.AddScoped<ITrackingNumberGenerator, TrackingNumberGenerator>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<ITransportRepository, TransportRepository>();
        services.AddScoped<ITerminalRepository, TerminalRepository>();
        
        return services;
    }
}