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

    public PackageService(
        UserManager<User> userManager,
        IUserContext currentUser,
        IValidator<CreatePackageRequestModel> validator,
        ITrackingNumberGenerator tracking,
        IPackageRepository packageRepository)
    {
        _userManager = userManager;
        _currentUser = currentUser;
        _validator = validator;
        _tracking = tracking;
        _packageRepository = packageRepository;
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
        if(!await _packageRepository.TerminalExistsAsync(requestModel.OriginTerminalId, ct))
            throw new ArgumentException("Origin terminal not found");
        
        if(!await _packageRepository.TerminalExistsAsync(requestModel.DestinationTerminalId, ct))
            throw new ArgumentException("Destination terminal not found");
        
        // transport validation
        if (requestModel.TransportId is not null)
        {
            if(!await _packageRepository.TransportExistsAsync(requestModel.TransportId.Value, ct))
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
}