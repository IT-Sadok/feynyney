using LogisticsApp.Application;
using LogisticsApp.Application.Terminals;
using LogisticsApp.Application.Transports;
using LogisticsApp.DTO;
using Microsoft.AspNetCore.Authorization;

namespace LogisticsApp.Extensions;

public static class TerminalEndpointsExtension
{
    public static WebApplication MapTerminalsEndpoints(this WebApplication app)
    {
        app.MapPost(Routes.Terminals,[Authorize(Roles = "Admin")] async (
            ITerminalService service,
            CreateTerminalRequestModel requestModel,
            CancellationToken ct) =>
        {
            var created = await service.CreateTerminalAsync(requestModel, ct);
            return Results.Created($"/terminals/{created.Id}", created);
        });
        
        return app;
    }
}