using FluentValidation;
using LogisticsApp.Application.Repositories;
using LogisticsApp.DTO;
using LogisticsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Application.Transports;

public class TransportService : ITransportService
{
    private readonly ITransportRepository _transportRepository;
    private readonly IValidator<CreateTransportRequestModel> _validator;

    public TransportService(
        ITransportRepository transportRepository,
        IValidator<CreateTransportRequestModel> validator)
    {
        _transportRepository = transportRepository;
        _validator = validator;
    }

    public async Task<TransportResponseModel> CreateTransportAsync(CreateTransportRequestModel requestModel,CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(requestModel, ct);
        
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