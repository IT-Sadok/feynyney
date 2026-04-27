using LogisticsApp.Application.Repositories;
using LogisticsApp.DTO;
using LogisticsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Application.Transports;

public class TransportService : ITransportService
{
    private ITransportRepository _transportRepository;

    public TransportService(ITransportRepository transportRepository)
    {
        _transportRepository = transportRepository;
    }

    public async Task<TransportResponseModel> CreateTransportAsync(CreateTransportRequestModel requestModel,CancellationToken ct)
    {
        var transport = new Transport
        {
            Type = requestModel.TransportType,
            Status = TransportStatus.Available,
        };

        await _transportRepository.AddTransportAsync(transport, ct);

        await _transportRepository.SaveChangesAsync(ct);

        return new TransportResponseModel(
            transport.Id,
            transport.Type,
            transport.Status
            );
    }
}