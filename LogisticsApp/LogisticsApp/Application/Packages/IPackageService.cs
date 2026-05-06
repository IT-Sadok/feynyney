using LogisticsApp.DTO;

namespace LogisticsApp.Application.Packages;

public interface IPackageService
{
    Task<PackageResponseModel> CreateAsync(CreatePackageRequestModel req, CancellationToken ct);
    Task<List<PackageDetailedResponseModel>> GetAllPackagesAsync(CancellationToken ct);
    Task<List<PackageResponseModel>> GetMyIncomingPackagesAsync(CancellationToken ct);
    Task<List<PackageResponseModel>> GetMySentPackagesAsync(CancellationToken ct);
    Task<PackageResponseModel> GetPackageByTrackingNumberAsync(string trackingNumber, CancellationToken ct);
    Task ReceivePackageAsync(ReceivePackageRequestModel requestModel, CancellationToken ct);
}