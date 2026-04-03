namespace LogisticsApp.Application.Repositories;

public interface ITransportRepository
{
    Task<bool> ExistsAsync(int id);
}