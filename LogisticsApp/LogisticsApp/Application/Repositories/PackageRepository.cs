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

    public Task SaveAsync(CancellationToken ct)
    {
        return _dbContext.SaveChangesAsync(ct);
    }

    public Task<List<Package>> GetAllPackagesAsync(CancellationToken ct)
    {
        return _dbContext.Packages
            .Include(x => x.SenderUser)
            .Include(x=> x.RecipientUser)
            .ToListAsync(ct);
    }

    public Task<List<Package>> GetIncomingPackagesAsync(string userId, CancellationToken ct)
    {
        return _dbContext.Packages.Where(p => p.RecipientUserId == userId).ToListAsync(ct); 
    }
    
    public Task<List<Package>> GetSentPackagesAsync(string userId, CancellationToken ct)
    {
        return _dbContext.Packages.Where(p => p.SenderUserId == userId).ToListAsync(ct); 
    }

    public Task<Package?> GetPackageByTrackingNumberAsync(string trackingNumber, CancellationToken ct)
    {
        return _dbContext.Packages.FirstOrDefaultAsync(p => p.TrackingNumber == trackingNumber, ct);
    }
    
    public Task<List<Package>> GetPackagesByIdsAsync(List<int> ids,CancellationToken ct)
    {
        return _dbContext.Packages.Where(p => ids.Contains(p.Id)).ToListAsync(ct);
    }
}