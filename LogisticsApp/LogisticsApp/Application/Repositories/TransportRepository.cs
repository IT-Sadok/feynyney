using LogisticsApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Application.Repositories;

public class TransportRepository : ITransportRepository
{
    private AppDbContext _dbContext;

    public TransportRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbContext.Transports.AnyAsync(t => t.Id == id);
    }
}