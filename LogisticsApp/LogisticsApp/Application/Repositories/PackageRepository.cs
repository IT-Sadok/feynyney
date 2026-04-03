using LogisticsApp.Data;
using LogisticsApp.Models;

namespace LogisticsApp.Application.Repositories;

public class PackageRepository : IPackageRepository
{
    private AppDbContext _dbContext;

    public PackageRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Package package)
    {
        await _dbContext.Packages.AddAsync(package);
    }
}