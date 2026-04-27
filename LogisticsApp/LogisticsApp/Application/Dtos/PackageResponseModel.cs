using LogisticsApp.Models;

namespace LogisticsApp.DTO;

public record PackageResponseModel(
    string packageName,
    int Id,
    string TrackingNumber,
    PackageStatus Status,
    DateTime SentAt
);
