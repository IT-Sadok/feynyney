using LogisticsApp.Application;
using LogisticsApp.Application.Packages;
using LogisticsApp.Application.Transports;
using LogisticsApp.DTO;
using Microsoft.AspNetCore.Authorization;

namespace LogisticsApp.Extensions;

public static class TransportEndpointsExtension
{
    public static WebApplication MapTransportEndpoints(this WebApplication app)
    {
        app.MapPost(Routes.Transports,[Authorize(Roles = "Admin")] async (
            ITransportService service,
            CreateTransportRequestModel requestModel,
            CancellationToken ct) =>
        {
            var created = await service.CreateTransportAsync(requestModel, ct);
            return Results.Created($"/transports/{created.Id}", created);
        });
        
        app.MapGet(Routes.AllTransports, [Authorize(Roles = "Admin")] async (
            ITransportService transportService,
            CancellationToken ct) =>
        {
            return Results.Ok(await transportService.GetAllTransportsAsync(ct));
        });
        
        app.MapPatch(Routes.MarkTransportAvailable, [Authorize(Roles = "Admin")] async (
            ITransportService transportService,
            MarkTransportAvailableRequestModel requestModel,
            CancellationToken ct) =>
        {
            await transportService.MarkTransportAvailableAsync(requestModel, ct);
            return Results.Ok("Transport is marked as available!");
        });
        
        app.MapPatch(Routes.MarkTransportUnavailable, [Authorize(Roles = "Admin")] async (
            ITransportService transportService,
            MarkTransportUnavailableRequestModel requestModel,
            CancellationToken ct) =>
        {
            await transportService.MarkTransportUnavailableAsync(requestModel, ct);
            return Results.Ok("Transport is marked as unavailable!");
        });
        
        return app;
    }
}