using FluentValidation;
using LogisticsApp.Application.Validation;
using LogisticsApp.Data;
using LogisticsApp.Extensions;
using LogisticsApp.Workers;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddDatabase(builder.Configuration)
            .AddJwtAuthentication(builder.Configuration)
            .AddAppIdentity()
            .AddApplication()
            .AddSeedSettings(builder.Configuration)
            .AddValidatorsFromAssemblyContaining<PackageModelValidation>()
            .AddAuthorization()
            .AddSwagger()
            .AddHostedService<TransportAssignmentWorker>();
            

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();
        
        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
        app.MapPackageEndpoints();
        app.MapTransportEndpoints();
        app.MapTerminalsEndpoints();

        await app.ApplyMigrationAsync();
        
        await app.SeedIdentityAsync();
        
        app.MapGet("/", () => Results.Ok("Logistics API is running"));
        
        await app.RunAsync();
    }
}