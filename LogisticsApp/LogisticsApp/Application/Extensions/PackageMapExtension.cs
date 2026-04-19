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

            TransportId = requestModel.TransportId,

            Status = PackageStatus.Sent,
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
            package.Status,
            package.SentAt);
    }
}