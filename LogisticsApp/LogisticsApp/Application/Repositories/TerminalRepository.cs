using LogisticsApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Application.Repositories;

public class TerminalRepository : ITerminalRepository
{
    private AppDbContext _dbContext;

    public TerminalRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbContext.Terminals.AnyAsync(t => t.Id == id);
    }
}