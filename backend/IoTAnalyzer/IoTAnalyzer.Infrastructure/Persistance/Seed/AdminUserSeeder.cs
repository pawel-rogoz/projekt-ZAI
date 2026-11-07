using IoTAnalyzer.Core.Entities;
using IoTAnalyzer.Core.Interfaces;
using IoTAnalyzer.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IoTAnalyzer.Infrastructure.Persistance.Seed;

public static class AdminUserSeeder
{
    public static async Task SeedAdminUserAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IoTAnalyzerDbContext>();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<AuthOptions>>().Value;
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(AdminUserSeeder));

        if (await dbContext.Users.AnyAsync(cancellationToken))
            return;

        if (string.IsNullOrWhiteSpace(options.AdminPassword))
        {
            logger.LogWarning("Brak użytkowników w bazie i nie ustawiono Auth__AdminPassword - konto administratora nie zostało utworzone.");
            return;
        }

        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        dbContext.Users.Add(new User(options.AdminUsername, passwordHasher.Hash(options.AdminPassword)));
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Utworzono konto administratora {Username}.", options.AdminUsername);
    }
}
