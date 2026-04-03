namespace LogisticsApp.Application.Packages;

public class TrackingNumberGenerator : ITrackingNumberGenerator
{
    public string New()
    {
        return $"PKG {Guid.NewGuid():N}";
    }
}