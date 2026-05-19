using LogisticsApp.DTO;
using LogisticsApp.Models;

namespace LogisticsApp.Extensions;

public static class TransportMapExtension
{
    public static TransportResponseModel ToTransportResponseModel(this Transport transport)
    {
        return new TransportResponseModel(
            transport.Id,
            transport.Type,
            transport.Status switch
            {
                TransportStatus.Available => "available",
                TransportStatus.InTransit => "in_transit",
                TransportStatus.Unavailable => "unavailable",
                _ => throw new ArgumentOutOfRangeException(nameof(transport.Status), transport.Status, null)
            });
    }
}