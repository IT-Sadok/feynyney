using LogisticsApp.DTO;
using LogisticsApp.Models;

namespace LogisticsApp.Extensions;

public static class PackageMapExtension
{
    public static Package ToPackage(
        this CreatePackageRequestModel requestModel,
        UserContextModel sender,
        string trackingNumber,
        DateTime now,
        User resipient
    )
    {
        return new Package()
        {
            Name = requestModel.Name,
            Weight = requestModel.Weight,
            TrackingNumber = trackingNumber,

            SenderUserId = sender.UserId,
            RecipientUserId = resipient.Id,

            OriginTerminalId = requestModel.OriginTerminalId,
            DestinationTerminalId = requestModel.DestinationTerminalId,

            TransportId = null,

            Status = PackageStatus.Created,
            SentAt = now,
            DeliveredAt = null
        };
    }

    public static PackageResponseModel ToPackageResponseModel(this Package package)
    {
        return new PackageResponseModel(
            package.Name,
            package.Id,
            package.TrackingNumber,
            package.Status.ToApiString(),
            package.SentAt,
            package.DeliveredAt);
    }
    
    public static PackageDetailedResponseModel ToPackageDetailedResponseModel(this Package package)
    {
        return new PackageDetailedResponseModel(
            package.Name,
            package.Id,
            package.SenderUser.Email ?? "",
            package.RecipientUser.Email ?? "",
            package.TrackingNumber,
            package.Status.ToApiString(),
            package.TransportId,
            package.SentAt,
            package.DeliveredAt);
    }

    public static string ToApiString(this PackageStatus status)
    {
        return status switch
        {
            PackageStatus.Created => "created",
            PackageStatus.WaitingForTransport => "waiting_for_transport",
            PackageStatus.InTransit => "in_transit",
            PackageStatus.Delivered => "delivered",
            PackageStatus.Received => "received",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}