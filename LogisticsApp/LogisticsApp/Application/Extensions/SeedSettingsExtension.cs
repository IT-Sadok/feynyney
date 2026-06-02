using LogisticsApp.Application.Settings;

namespace LogisticsApp.Extensions;

public static class SeedSettingsExtension
{
    public static IServiceCollection AddSeedSettings(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<SeedSettings>(config.GetSection("SeedSettings"));

        return services;
    }
}