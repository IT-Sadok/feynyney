namespace LogisticsApp.Models;

public enum PackageStatus
{
    Created = 1,
    WaitingForTransport = 2,
    InTransit = 3,
    Delivered = 4,
    Received = 5
}