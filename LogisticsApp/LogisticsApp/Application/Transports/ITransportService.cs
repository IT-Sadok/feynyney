using LogisticsApp.DTO;

namespace LogisticsApp.Application.Transports;

public interface ITransportService
{
    Task<TransportResponseModel> CreateTransportAsync(CreateTransportRequestModel requestModel, CancellationToken ct);
    Task<List<TransportResponseModel>> GetAllTransportsAsync(CancellationToken ct);
    Task MarkTransportUnavailableAsync(MarkTransportUnavailableRequestModel requestModel, CancellationToken ct);
    Task MarkTransportAvailableAsync(MarkTransportAvailableRequestModel requestModel, CancellationToken ct);
}