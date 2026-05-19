using LogisticsApp.DTO;

namespace LogisticsApp.Application.Packages;

public interface IPackageService
{
    Task<PackageResponseModel> CreateAsync(CreatePackageRequestModel req, CancellationToken ct);
    Task<List<PackageDetailedResponseModel>> GetAllPackagesAsync(CancellationToken ct);
    Task<List<PackageResponseModel>> GetMyIncomingPackagesAsync(CancellationToken ct);
    Task<List<PackageResponseModel>> GetMySentPackagesAsync(CancellationToken ct);
    Task<PackageResponseModel> GetPackageByTrackingNumberAsync(string trackingNumber, CancellationToken ct);
    Task ReceivePackagesAsync(ReceivePackagesRequestModel requestModel, CancellationToken ct);
    Task ApprovePackagesAsync(ApprovePackagesRequestModel requestModel, CancellationToken ct);
    Task MarkPackagesDeliveredAsync(MarkPackagesDeliveredRequestModel requestModel, CancellationToken ct);
    Task TryAssignWaitingPackagesAsync(CancellationToken ct);
}