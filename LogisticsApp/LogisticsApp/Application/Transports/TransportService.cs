using FluentValidation;
using LogisticsApp.Application.Repositories;
using LogisticsApp.DTO;
using LogisticsApp.Extensions;
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
        
        return transport.ToTransportResponseModel();
    }

    public async Task<List<TransportResponseModel>> GetAllTransportsAsync(CancellationToken ct)
    {
        var transports = await _transportRepository.GetAllTransportsAsync(ct);

        return transports
            .Select(x => x.ToTransportResponseModel())
            .ToList();
    }

    public async Task MarkTransportUnavailableAsync(MarkTransportUnavailableRequestModel requestModel, CancellationToken ct)
    {
        var transports = await _transportRepository.GetTransportsByIdsAsync(requestModel.Ids, ct);

        if (transports.Count != requestModel.Ids.Count)
            throw new ArgumentException("Some transports are missing!");

        foreach (var transport in transports)
        {
            if(transport.Status == TransportStatus.Available)
                transport.Status = TransportStatus.Unavailable;
            else
            {
                throw new ArgumentException("Transport status should be available!");
            }
        }
        
        await _transportRepository.SaveChangesAsync(ct);
    }
    
    public async Task MarkTransportAvailableAsync(MarkTransportAvailableRequestModel requestModel, CancellationToken ct)
    {
        var transports = await _transportRepository.GetTransportsByIdsAsync(requestModel.Ids, ct);

        if (transports.Count != requestModel.Ids.Count)
            throw new ArgumentException("Some transports are missing!");

        foreach (var transport in transports)
        {
            if(transport.Status == TransportStatus.Unavailable)
                transport.Status = TransportStatus.Available;
            else
            {
                throw new ArgumentException("Transport status should be unavailable!");
            }
        }
        
        await _transportRepository.SaveChangesAsync(ct);
    }
}