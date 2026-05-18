using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Application.Repositories;

public class TerminalRepository : ITerminalRepository
{
    private AppDbContext _dbContext;
    
    public TerminalRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddTerminalAsync(Terminal terminal, CancellationToken ct)
    {
        await _dbContext.Terminals.AddAsync(terminal, ct);
    }
    
    public async Task<bool> TerminalExistsAsync(int id, CancellationToken ct)
    {
        return await _dbContext.Terminals.AnyAsync(t => t.Id == id, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}