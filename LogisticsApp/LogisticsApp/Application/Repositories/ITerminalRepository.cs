namespace LogisticsApp.Application.Repositories;

public interface ITerminalRepository
{
    Task<bool> ExistsAsync(int id);
}