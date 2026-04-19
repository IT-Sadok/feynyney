using LogisticsApp.DTO;

namespace LogisticsApp.Application.Packages;

public interface IPackageService
{
    Task<PackageResponseModel> CreateAsync(CreatePackageRequestModel req, CancellationToken ct);
    Task<List<PackageResponseModel>> GetMyPackagesAsync(CancellationToken ct);
}