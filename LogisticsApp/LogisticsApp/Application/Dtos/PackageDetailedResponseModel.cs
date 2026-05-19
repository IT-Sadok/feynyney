using LogisticsApp.Models;

namespace LogisticsApp.DTO;

public record PackageDetailedResponseModel(
    string PackageName,
    int Id,
    string SenderEmail,
    string RecipientEmail,
    string TrackingNumber,
    string Status,
    int? TransportId,
    DateTime SentAt,
    DateTime? DeliveredAt);
