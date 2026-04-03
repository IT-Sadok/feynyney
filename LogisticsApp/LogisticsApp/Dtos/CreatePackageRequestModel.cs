namespace LogisticsApp.DTO;

public record CreatePackageRequestModel(
    string Name,
    decimal Weight,
    string RecipientEmail,
    int OriginTerminalId,
    int DestinationTerminalId,
    int? TransportId
);