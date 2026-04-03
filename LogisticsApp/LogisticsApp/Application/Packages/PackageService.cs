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
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IUserContext _currentUser;
    private readonly IValidator<CreatePackageRequestModel> _validator;
    private readonly ITrackingNumberGenerator _tracking;
    private readonly ITerminalRepository _terminalRepository;
    private readonly ITransportRepository _transportRepository;
    private readonly IPackageRepository _packageRepository;

    public PackageService(
        AppDbContext dbContext,
        UserManager<User> userManager,
        IUserContext currentUser,
        IValidator<CreatePackageRequestModel> validator,
        ITrackingNumberGenerator tracking,
        ITerminalRepository terminalRepository,
        ITransportRepository transportRepository,
        IPackageRepository packageRepository)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _currentUser = currentUser;
        _validator = validator;
        _tracking = tracking;
        _terminalRepository = terminalRepository;
        _transportRepository = transportRepository;
        _packageRepository = packageRepository;
    }
    
    public async Task<PackageResponseModel> CreateAsync(CreatePackageRequestModel requestModel)
    {
        // basic validation
        var validationResult =  await _validator.ValidateAsync(requestModel);
        if(!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // current user is a sender
        var sender = _currentUser.User;

        // recipient validation
        var recipient = await _userManager.FindByEmailAsync(requestModel.RecipientEmail);
        if (recipient is null)
            throw new ArgumentException("Recipient not found");
        
        // terminal validation
        if(!await _terminalRepository.ExistsAsync(requestModel.OriginTerminalId))
            throw new ArgumentException("Origin terminal not found");
        
        if(!await _terminalRepository.ExistsAsync(requestModel.DestinationTerminalId))
            throw new ArgumentException("Destination terminal not found");
        
        // transport validation
        if (requestModel.TransportId is not null)
        {
            if(!await _transportRepository.ExistsAsync(requestModel.TransportId.Value))
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
        await _packageRepository.AddAsync(package);

        await _dbContext.SaveChangesAsync();
        
        // response
        return new PackageResponseModel(
            package.Id,
            package.TrackingNumber,
            package.Status,
            package.SentAt
        );
    }
}