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
}