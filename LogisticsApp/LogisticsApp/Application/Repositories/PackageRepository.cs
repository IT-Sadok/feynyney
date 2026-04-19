using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Application.Repositories;

public class PackageRepository : IPackageRepository
{
    private AppDbContext _dbContext;

    public PackageRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddPackageAsync(Package package)
    {
        await _dbContext.Packages.AddAsync(package);
    }
    
    public async Task<bool> TerminalExistsAsync(int id)
    {
        return await _dbContext.Terminals.AnyAsync(t => t.Id == id);
    }
    
    public async Task<bool> TransportExistsAsync(int id)
    {
        return await _dbContext.Transports.AnyAsync(t => t.Id == id);
    }

    public Task SaveAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
}