using LogisticsApp.Data;
using LogisticsApp.Models;

namespace LogisticsApp.Application.Repositories;

public interface IPackageRepository
{
    Task AddPackageAsync(Package package);
    Task<bool> TerminalExistsAsync(int id);
    Task<bool> TransportExistsAsync(int id);
    Task SaveAsync();
}