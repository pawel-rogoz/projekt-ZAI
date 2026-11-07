using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IoTAnalyzer.Infrastructure.Persistance;

public static class DatabaseMigrator
{
    public static async Task ApplyMigrationsAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IoTAnalyzerDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
