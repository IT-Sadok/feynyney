using LogisticsApp.Models;

namespace LogisticsApp.Application.Repositories;

public interface ITransportRepository
{
    Task<bool> TransportExistsAsync(int id, CancellationToken ct);
    Task AddTransportAsync(Transport transport, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task<List<Transport>> GetAllTransportsAsync(CancellationToken ct);
    Task<List<Transport>> GetTransportsByIdsAsync(List<int> ids, CancellationToken ct);
    Task<Transport?> GetTransportAsync(int id, CancellationToken ct);
    Task<List<Transport>> GetAvailableTransportsAsync(CancellationToken ct);
}