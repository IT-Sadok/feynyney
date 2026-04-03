using LogisticsApp.Data;
using LogisticsApp.Models;

namespace LogisticsApp.Application.Repositories;

public interface IPackageRepository
{
    Task AddAsync(Package package);
}