namespace LogisticsApp.DTO;

public record CreatePackageRequestModel(
    string Name,
    double Weight,
    string RecipientEmail,
    int OriginTerminalId,
    int DestinationTerminalId
);