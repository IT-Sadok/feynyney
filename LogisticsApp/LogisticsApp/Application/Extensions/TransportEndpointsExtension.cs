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
        app.MapPost(Routes.Transports, async (
            ITransportService service,
            CreateTransportRequestModel requestModel,
            CancellationToken ct) =>
        {
            var created = await service.CreateTransportAsync(requestModel, ct);
            return Results.Created($"/transports/{created.Id}", created);
        });
        
        return app;
    }
}