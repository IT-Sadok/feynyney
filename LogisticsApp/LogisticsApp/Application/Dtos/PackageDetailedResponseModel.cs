using LogisticsApp.Models;

namespace LogisticsApp.DTO;

public record PackageDetailedResponseModel(
    string PackageName,
    int Id,
    string SenderEmail,
    string RecipientEmail,
    string TrackingNumber,
    PackageStatus Status,
    DateTime SentAt);
