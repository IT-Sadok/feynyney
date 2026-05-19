using LogisticsApp.Application;
using LogisticsApp.Application.Packages;
using LogisticsApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LogisticsApp.Extensions;

public static class PackageEndpointsExtension
{
    public static WebApplication MapPackageEndpoints(this WebApplication app)
    {
        app.MapPost(Routes.Packages, [Authorize] async (
            IPackageService service,
            CreatePackageRequestModel requestModel,
            CancellationToken ct) =>
            {
               var created = await service.CreateAsync(requestModel, ct);
               return Results.Created($"/packages/track/{created.TrackingNumber}", created);
            });
        
        app.MapGet(Routes.MyIncomingPackages, [Authorize] async (IPackageService packageService, CancellationToken ct) =>
        {
            return Results.Ok(await packageService.GetMyIncomingPackagesAsync(ct));
        });
        
        app.MapGet(Routes.MySentPackages, [Authorize] async (IPackageService packageService, CancellationToken ct) =>
        {
            return Results.Ok(await packageService.GetMySentPackagesAsync(ct));
        });
        
        app.MapGet(Routes.PackageNumber, async (
            IPackageService packageService,
            string trackingNumber,
            CancellationToken ct) =>
        {
            return Results.Ok(await packageService.GetPackageByTrackingNumberAsync(trackingNumber, ct));
        });
        
        app.MapGet(Routes.AllPackages, [Authorize(Roles = "Admin")] async (
            IPackageService packageService,
            CancellationToken ct) =>
        {
            return Results.Ok(await packageService.GetAllPackagesAsync(ct));
        });
        
        app.MapPatch(Routes.ReceivePackages, [Authorize(Roles = "Admin")] async (
            IPackageService packageService,
            ReceivePackagesRequestModel requestModel,
            CancellationToken ct) =>
        {
            await packageService.ReceivePackagesAsync(requestModel, ct);
            return Results.Ok("Package received!");
        });
        
        app.MapPatch(Routes.ApprovePackages, [Authorize(Roles = "Admin")] async (
            IPackageService packageService,
            ApprovePackagesRequestModel requestModel,
            CancellationToken ct) =>
        {
            await packageService.ApprovePackagesAsync(requestModel, ct);
            return Results.Ok("Package approved!");
        });
        
        app.MapPatch(Routes.DeliverPackages, [Authorize(Roles = "Admin")] async (
            IPackageService packageService,
            MarkPackagesDeliveredRequestModel requestModel,
            CancellationToken ct) =>
        {
            await packageService.MarkPackagesDeliveredAsync(requestModel, ct);
            return Results.Ok("Package delivered!");
        });
        
        return app;
    }
    
    
}