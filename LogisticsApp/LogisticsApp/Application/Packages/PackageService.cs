using LogisticsApp.Application.Users;
using LogisticsApp.Data;
using LogisticsApp.DTO;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Application.Packages;

public class PackageService : IPackageService
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IUserContext _currentUser;
    private readonly ITrackingNumberGenerator _tracking;

    public PackageService(
        AppDbContext dbContext,
        UserManager<User> userManager,
        IUserContext currentUser,
        ITrackingNumberGenerator tracking)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _currentUser = currentUser;
        _tracking = tracking;
    }
    
    public async Task<PackageResponseModel> Create(CreatePackageRequestModel requestModel)
    {
        // basic validation
        if (string.IsNullOrWhiteSpace(requestModel.Name))
            throw new ArgumentException("Name is required");
        if(requestModel.Weight <= 0)
            throw new ArgumentException("Weight cannot be less or equal 0");
        if(requestModel.OriginTerminalId == requestModel.DestinationTerminalId)
            throw new ArgumentException("Original and destination terminals must be different");

        // current user is a sender
        var senderId = _currentUser.UserId;

        // recipient validation
        var recipient = await _userManager.FindByEmailAsync(requestModel.RecipientEmail);
        if (recipient is null)
            throw new ArgumentException("Recipient not found");
        
        // terminal validation
        var originExists = await _dbContext.Terminals.AnyAsync(t => t.Id == requestModel.OriginTerminalId);
        if (!originExists)
            throw new ArgumentException("Origin terminal not found");
        
        var destinationExists = await _dbContext.Terminals.AnyAsync(t => t.Id == requestModel.DestinationTerminalId);
        if (!destinationExists)
            throw new ArgumentException("Destination terminal not found");
        
        // transport validation
        if (requestModel.TransportId is not null)
        {
            var transportExists = await _dbContext.Transports.AnyAsync(t => t.Id == requestModel.TransportId);
            if(!transportExists)
                throw new ArgumentException("Transport not found");
        }
        
        // generate tracking number
        string trackingNumber = "";

        for (var attemt = 0; attemt < 3; attemt++)
        {
            trackingNumber = _tracking.New();
            var exists = await _dbContext.Packages.AnyAsync(p => p.TrackingNumber == trackingNumber);
            if(!exists) break;
        }
        
        var now = DateTime.UtcNow;
        
        // create entity
        var package = new Package
        {
            Name = requestModel.Name,
            Weight = requestModel.Weight,
            TrackingNumber = trackingNumber,

            SenderUserId = senderId,
            RecipientUserId = recipient.Id,

            OriginTerminalId = requestModel.OriginTerminalId,
            DestinationTerminalId = requestModel.DestinationTerminalId,

            TransportId = requestModel.TransportId,

            Status = PackageStatus.Sent,
            SentAt = now,
            DeliveredAt = null
        };
        
        // save
        _dbContext.Packages.Add(package);
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