using LogisticsApp.Application.Users;
using LogisticsApp.Data;
using LogisticsApp.DTO;
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

    public async Task AddPackageAsync(Package package, CancellationToken ct)
    {
        await _dbContext.Packages.AddAsync(package, ct);
    }
    
    public async Task<bool> TerminalExistsAsync(int id, CancellationToken ct)
    {
        return await _dbContext.Terminals.AnyAsync(t => t.Id == id, ct);
    }
    
    public async Task<bool> TransportExistsAsync(int id, CancellationToken ct)
    {
        return await _dbContext.Transports.AnyAsync(t => t.Id == id, ct);
    }

    public Task SaveAsync(CancellationToken ct)
    {
        return _dbContext.SaveChangesAsync(ct);
    }

    public Task<List<Package>> GetPackagesByRecipientIdAsync(string userId, CancellationToken ct)
    {
        return _dbContext.Packages.Where(p => p.RecipientUserId == userId).ToListAsync(ct); 
    }

    public Task<Package?> GetPackageByTrackingNumberAsync(string trackingNumber, CancellationToken ct)
    {
        return _dbContext.Packages.FirstOrDefaultAsync(p => p.TrackingNumber == trackingNumber, ct);
    }
}