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
            ReceivePackageRequestModel requestModel,
            CancellationToken ct) =>
        {
            await packageService.ReceivePackageAsync(requestModel, ct);
            return Results.Ok("Package received!");
        });
        
        return app;
    }
    
    
}