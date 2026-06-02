using FluentValidation;
using LogisticsApp.Application.Repositories;
using LogisticsApp.Application.Users;
using LogisticsApp.Data;
using LogisticsApp.DTO;
using LogisticsApp.Extensions;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Application.Packages;

public class PackageService : IPackageService
{
    private readonly UserManager<User> _userManager;
    private readonly IUserContext _currentUser;
    private readonly IValidator<CreatePackageRequestModel> _validator;
    private readonly ITrackingNumberGenerator _tracking;
    private readonly IPackageRepository _packageRepository;
    private readonly ITransportRepository _transportRepository;
    private readonly ITerminalRepository _terminalRepository;
    

    public PackageService(
        UserManager<User> userManager,
        IUserContext currentUser,
        IValidator<CreatePackageRequestModel> validator,
        ITrackingNumberGenerator tracking,
        IPackageRepository packageRepository,
        ITransportRepository transportRepository,
        ITerminalRepository terminalRepository)
    {
        _userManager = userManager;
        _currentUser = currentUser;
        _validator = validator;
        _tracking = tracking;
        _packageRepository = packageRepository;
        _transportRepository = transportRepository;
        _terminalRepository = terminalRepository;
    }
    
    public async Task<PackageResponseModel> CreateAsync(CreatePackageRequestModel requestModel, CancellationToken ct)
    {
        // basic validation
        await _validator.ValidateAndThrowAsync(requestModel, ct);

        // current user is a sender
        var sender = _currentUser.User;

        // recipient validation
        var recipient = await _userManager.FindByEmailAsync(requestModel.RecipientEmail);
        if (recipient is null)
            throw new ArgumentException("Recipient not found");
        
        // terminal validation
        if(!await _terminalRepository.TerminalExistsAsync(requestModel.OriginTerminalId, ct))
            throw new ArgumentException("Origin terminal not found");
        
        if(!await _terminalRepository.TerminalExistsAsync(requestModel.DestinationTerminalId, ct))
            throw new ArgumentException("Destination terminal not found");
        
        // transport validation
        // if (requestModel.TransportId is not null)
        // {
        //     if(!await _transportRepository.TransportExistsAsync(requestModel.TransportId.Value, ct))
        //         throw new ArgumentException("Transport not found");
        // }
        
        // generate tracking number
        string trackingNumber = _tracking.GenerateTrackingNumber();
        
        var now = DateTime.UtcNow;
        
        // create entity
        var package = requestModel.ToPackage(
            sender,
            trackingNumber,
            now,
            recipient
            );
        
        // save
        await _packageRepository.AddPackageAsync(package, ct);

        await _packageRepository.SaveAsync(ct);
        
        // response
        return package.ToPackageResponseModel();
    }

    public async Task<List<PackageDetailedResponseModel>> GetAllPackagesAsync(CancellationToken ct)
    {
        var packages  = await _packageRepository.GetAllPackagesAsync(ct);
        
        return packages
            .Select(x => x.ToPackageDetailedResponseModel())
            .ToList();
    }

    public async Task<List<PackageResponseModel>> GetMyIncomingPackagesAsync(CancellationToken ct)
    {
        var currentUser = _currentUser.User;
        
        var packages = await _packageRepository.GetIncomingPackagesAsync(currentUser.UserId, ct);
        
        return packages
            .Select(x => x.ToPackageResponseModel())
            .ToList();
    }
    
    public async Task<List<PackageResponseModel>> GetMySentPackagesAsync(CancellationToken ct)
    {
        var currentUser = _currentUser.User;
        
        var packages = await _packageRepository.GetSentPackagesAsync(currentUser.UserId, ct);
        
        return packages.
            Select(x => x.ToPackageResponseModel())
            .ToList();
    }

    public async Task<PackageResponseModel> GetPackageByTrackingNumberAsync(string trackingNumber, CancellationToken ct)
    {
        var package = await _packageRepository.GetPackageByTrackingNumberAsync(trackingNumber, ct);

        if (package is null)
        {
            throw new ArgumentException("Package not found");
        }
        
        return package.ToPackageResponseModel();
    }

    public async Task ReceivePackagesAsync(ReceivePackagesRequestModel requestModel ,CancellationToken ct)
    {
        var packages = await _packageRepository.GetPackagesByIdsAsync(requestModel.Ids, ct);

        if (packages.Count != requestModel.Ids.Count) 
            throw new ArgumentException("Some packages were not found!");
        
        foreach (var package in packages)
        {
            switch (package.Status) 
            {
                case PackageStatus.Received:
                    throw new ArgumentException("Package is already received!");
                
                // case PackageStatus.Sent: //TODO Make admin approve package status to "In Transit"
                case PackageStatus.InTransit:
                    throw new ArgumentException("Package is not delivered yet!");
            }
                
            package.Status = PackageStatus.Received;
        }
        
        await _packageRepository.SaveAsync(ct);
    }

    public async Task ApprovePackagesAsync(ApprovePackagesRequestModel requestModel, CancellationToken ct)
    {
        var packages = await _packageRepository.GetPackagesByIdsAsync(requestModel.Ids, ct);

        if (packages.Count != requestModel.Ids.Count)
            throw new ArgumentException("Some packages were not found!");
        
        var availableTransports = await  _transportRepository.GetAvailableTransportsAsync(ct);
        
        var assignedCount = Math.Min(packages.Count, availableTransports.Count);

        foreach (var package in packages)
        {
            if(package.Status !=  PackageStatus.Created)
                throw new ArgumentException("Only packages with Created status can be approved.");
        }
            
        for (int i = 0; i < assignedCount; i++)
        {
            packages[i].Status = PackageStatus.InTransit;
            packages[i].TransportId = availableTransports[i].Id;
            availableTransports[i].Status = TransportStatus.InTransit;
        }

        for (int i = assignedCount; i < packages.Count; i++)
        {
            packages[i].Status = PackageStatus.WaitingForTransport;
            packages[i].TransportId = null;
        }
        
        await _packageRepository.SaveAsync(ct);
    }

    public async Task MarkPackagesDeliveredAsync(MarkPackagesDeliveredRequestModel requestModel, CancellationToken ct)
    {
        var packages = await _packageRepository.GetPackagesByIdsWithTransportAsync(requestModel.Ids, ct);

        if (packages.Count != requestModel.Ids.Count) 
            throw new ArgumentException("Some packages were not found!");

        foreach (var package in packages)
        {
            if(package.Status !=  PackageStatus.InTransit)
                throw new ArgumentException("Only packages with InTransit status can be marked as Delivered.");
            
            package.Status = PackageStatus.Delivered;
            package.DeliveredAt = DateTime.UtcNow;

            if(package.Transport == null)
                throw new ArgumentException("InTransit package must have an assigned transport.");
            
            package.Transport.Status = TransportStatus.Available;
        }
        
        await _packageRepository.SaveAsync(ct);
    }

    public async Task TryAssignWaitingPackagesAsync(CancellationToken ct)
    {
        var packages = await _packageRepository.GetWaitingPackagesAsync(ct);
        
        var availableTransports = await  _transportRepository.GetAvailableTransportsAsync(ct);
        
        var assignedCount = Math.Min(packages.Count, availableTransports.Count);
            
        for (int i = 0; i < assignedCount; i++)
        {
            packages[i].Status = PackageStatus.InTransit;
            packages[i].TransportId = availableTransports[i].Id;
            availableTransports[i].Status = TransportStatus.InTransit;
        }
        
        await _packageRepository.SaveAsync(ct);
    }
}