namespace LogisticsApp.Models;

public class Package
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public decimal Weight { get; set; }

    public string TrackingNumber { get; set; } = "";

    // Users (Identity)
    public string SenderUserId { get; set; } = "";
    public User SenderUser { get; set; } = null!;

    public string RecipientUserId { get; set; } = "";
    public User RecipientUser { get; set; } = null!;

    // Terminals
    public int OriginTerminalId { get; set; }
    public Terminal OriginTerminal { get; set; } = null!;

    public int DestinationTerminalId { get; set; }
    public Terminal DestinationTerminal { get; set; } = null!;

    // Transport
    public int? TransportId { get; set; }
    public Transport? Transport { get; set; }

    public PackageStatus Status { get; set; }

    public DateTime SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
}