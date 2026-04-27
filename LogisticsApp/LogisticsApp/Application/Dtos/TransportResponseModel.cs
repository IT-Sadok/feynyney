using LogisticsApp.Models;

namespace LogisticsApp.DTO;

public record TransportResponseModel(int Id, string Type, TransportStatus Status);
