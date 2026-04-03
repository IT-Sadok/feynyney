using LogisticsApp.Application.Packages;
using LogisticsApp.DTO;
using Microsoft.AspNetCore.Authorization;

namespace LogisticsApp.Extensions;

public static class PackageEndpointsExtension
{
    public static WebApplication MapPackageEndpoints(this WebApplication app)
    {
        app.MapPost("/packages", [AuthorizeAttribute] async (IPackageService service, CreatePackageRequestModel requestModel) =>
            {
               var created = await service.Create(requestModel);
               return Results.Created($"/packages/track/{created.TrackingNumber}", created);
            });
        
        return app;
    }
}