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
        await _validator.ValidateAndThrowAsync(requestModel, cancellationToken: ct);

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
        if (requestModel.TransportId is not null)
        {
            if(!await _transportRepository.TransportExistsAsync(requestModel.TransportId.Value, ct))
                throw new ArgumentException("Transport not found");
        }
        
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
        return new PackageResponseModel(
            package.Name,
            package.Id,
            package.TrackingNumber,
            package.Status,
            package.SentAt
        );
    }

    public async Task<List<PackageResponseModel>> GetMyIncomingPackagesAsync(CancellationToken ct)
    {
        var currentUser = _currentUser.User;
        
        var packages = await _packageRepository.GetIncomingPackagesAsync(currentUser.UserId, ct);
        
        return packages.
            Select(x => x.ToPackageResponseModel())
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

    public async Task ReceivePackageAsync(List<int> ids ,CancellationToken ct)
    {
        var currentUser = _currentUser.User;
        
        var packages = await _packageRepository.GetPackagesByIdsAsync(ids, ct);

        if (packages.Count == ids.Count)
        {
            foreach (var package in packages)
            {
                if (package.RecipientUserId != currentUser.UserId)
                {
                    throw new ArgumentException("Package does not belong to this user!");
                }
            
                switch (package.Status) 
                {
                    case PackageStatus.Received:
                        throw new ArgumentException("Package is already received!");
                
                    // case PackageStatus.Sent: //TODO Make admin approve package status to "In Transit"
                    case PackageStatus.InTransit:
                        throw new ArgumentException("Package is not delivered yet!");
                }
            }

            foreach (var package in packages)
            {
                package.Status = PackageStatus.Received;
            }
        }
        else
        {
            throw new ArgumentException("Some packages were not found!");
        }
        
        await _packageRepository.SaveAsync(ct);
    }

    public async Task ApprovePackageAsync(List<int> ids, CancellationToken ct)
    {
        var packages = await _packageRepository.GetPackagesByIdsAsync(ids, ct);

        if (packages.Count == ids.Count)
        {
            foreach (var package in packages)
            {
                if (package.Status == PackageStatus.InTransit)
                {
                    package.Status = PackageStatus.InTransit;
                }
            }
        }
    }
}