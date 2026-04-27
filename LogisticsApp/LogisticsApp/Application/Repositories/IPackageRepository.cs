using LogisticsApp.Data;
using LogisticsApp.Models;

namespace LogisticsApp.Application.Repositories;

public interface IPackageRepository
{
    Task AddPackageAsync(Package package, CancellationToken ct);
    Task<bool> TerminalExistsAsync(int id, CancellationToken ct);
    Task<bool> TransportExistsAsync(int id, CancellationToken ct);
    Task SaveAsync(CancellationToken ct);
    Task<List<Package>> GetIncomingPackagesAsync(string userId, CancellationToken ct);
    Task<List<Package>> GetSentPackagesAsync(string userId, CancellationToken ct);
    Task<Package?> GetPackageByTrackingNumberAsync(string trackingNumber, CancellationToken ct);
}