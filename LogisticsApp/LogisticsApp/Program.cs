using LogisticsApp.Extensions;

namespace LogisticsApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddDatabase(builder.Configuration)
            .AddJwtAuthentication(builder.Configuration)
            .AddAppIdentity()
            .AddApplication()
            .AddAuthorization()
            .AddSwagger();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();
        
        app.UseHttpsRedirection();
        app.UseSwaggerIfDev();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
        app.MapPackageEndpoints();
        
        app.Run();
    }
}