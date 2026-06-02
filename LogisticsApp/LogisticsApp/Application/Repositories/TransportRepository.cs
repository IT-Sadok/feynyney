using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Application.Repositories;

public class TransportRepository : ITransportRepository
{
    private AppDbContext _dbContext;
    
    public TransportRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddTransportAsync(Transport transport ,CancellationToken ct)
    {
        await _dbContext.Transports.AddAsync(transport, ct);
    }
    
    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
    
    public async Task<bool> TransportExistsAsync(int id, CancellationToken ct)
    {
        return await _dbContext.Transports.AnyAsync(t => t.Id == id, ct);
    }

    public async Task<List<Transport>> GetAllTransportsAsync(CancellationToken ct)
    {
        return await _dbContext.Transports.ToListAsync(ct);
    }
    
    public Task<List<Transport>> GetTransportsByIdsAsync(List<int> ids,CancellationToken ct)
    {
        return _dbContext.Transports
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(ct);
    }

    public async Task<Transport?> GetTransportAsync(int id, CancellationToken ct)
    {
        return await _dbContext.Transports.FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<List<Transport>> GetAvailableTransportsAsync(CancellationToken ct)
    {
        return await _dbContext.Transports.Where(t => t.Status == TransportStatus.Available).ToListAsync(ct);
    }
}