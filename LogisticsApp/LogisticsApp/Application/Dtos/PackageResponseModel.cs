using LogisticsApp.Models;

namespace LogisticsApp.DTO;

public record PackageResponseModel(
    string PackageName,
    int Id,
    string TrackingNumber,
    string Status,
    DateTime SentAt,
    DateTime? DeliveredAt
);
