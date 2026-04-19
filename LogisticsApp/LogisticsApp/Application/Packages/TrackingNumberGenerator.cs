namespace LogisticsApp.Application.Packages;

public class TrackingNumberGenerator : ITrackingNumberGenerator
{
    public string GenerateTrackingNumber()
    {
        return $"PKG {Guid.NewGuid():N}";
    }
}