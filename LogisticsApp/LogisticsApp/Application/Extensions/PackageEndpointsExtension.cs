using LogisticsApp.Application;
using LogisticsApp.Application.Packages;
using LogisticsApp.DTO;
using Microsoft.AspNetCore.Authorization;

namespace LogisticsApp.Extensions;

public static class PackageEndpointsExtension
{
    public static WebApplication MapPackageEndpoints(this WebApplication app)
    {
        app.MapPost(Routes.Packages, [AuthorizeAttribute] async (
            IPackageService service,
            CreatePackageRequestModel requestModel,
            CancellationToken ct) =>
            {
               var created = await service.CreateAsync(requestModel, ct);
               return Results.Created($"/packages/track/{created.TrackingNumber}", created);
            });
        
        app.MapGet(Routes.MyPackages, [Authorize] async (IPackageService packageService, CancellationToken ct) =>
        {
            return Results.Ok(await packageService.GetMyPackagesAsync(ct));
        });
        
        return app;
    }
    
    
}