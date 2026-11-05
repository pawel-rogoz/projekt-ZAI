using Ardalis.SharedKernel;
using IoTAnalyzer.Core.Interfaces;
using IoTAnalyzer.Infrastructure.Persistance.Repositories;
using IoTAnalyzer.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IoTAnalyzer.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<IoTAnalyzerDbContext>(options => options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention());

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddSingleton<IApiKeyService, ApiKeyService>();

        return services;
    }
}
