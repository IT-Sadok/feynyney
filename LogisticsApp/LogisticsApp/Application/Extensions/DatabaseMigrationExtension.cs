using LogisticsApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Extensions;

public static class DatabaseMigrationExtension
{
    public static async Task ApplyMigrationAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
        }
    }
}