namespace LogisticsApp.Application.Settings;

public class SeedSettings
{
    public AdminSeedSettings Admin {get; set;} = new();
    public List<string> Roles { get; set; } = new();
}