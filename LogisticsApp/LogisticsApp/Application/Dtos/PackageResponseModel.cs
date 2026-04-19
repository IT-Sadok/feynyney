using LogisticsApp.Models;

namespace LogisticsApp.DTO;

public record PackageResponseModel(
    int Id,
    string TrackingNumber,
    PackageStatus Status,
    DateTime SentAt
);
