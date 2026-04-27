using LogisticsApp.Models;

namespace LogisticsApp.Application.Repositories;

public interface ITransportRepository
{
    Task<bool> TransportExistsAsync(int id, CancellationToken ct);
    Task AddTransportAsync(Transport transport, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}