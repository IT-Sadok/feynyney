using LogisticsApp.Models;

namespace LogisticsApp.Application.Repositories;

public interface ITerminalRepository
{
    Task<bool> TerminalExistsAsync(int id, CancellationToken ct);
    Task AddTerminalAsync(Terminal terminal, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}