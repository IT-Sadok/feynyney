using LogisticsApp.DTO;

namespace LogisticsApp.Application.Packages;

public interface IPackageService
{
    Task<PackageResponseModel> Create(CreatePackageRequestModel req);
}