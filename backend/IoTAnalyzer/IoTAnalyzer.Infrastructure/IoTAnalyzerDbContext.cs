using IoTAnalyzer.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IoTAnalyzer.Infrastructure;

public class IoTAnalyzerDbContext(DbContextOptions<IoTAnalyzerDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<AuthSession> AuthSessions => Set<AuthSession>();
    public DbSet<Series> Series => Set<Series>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<Measurement> Measurements => Set<Measurement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IoTAnalyzerDbContext).Assembly);
    }
}
