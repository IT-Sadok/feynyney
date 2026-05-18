using LogisticsApp.DTO;

namespace LogisticsApp.Application.Transports;

public interface ITransportService
{
    Task<TransportResponseModel> CreateTransportAsync(CreateTransportRequestModel requestModel, CancellationToken ct);
}