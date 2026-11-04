using IoTAnalyzer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTAnalyzer.Infrastructure.Persistance.EfConfigs;

public class MeasurementConfig : IEntityTypeConfiguration<Measurement>
{
    public void Configure(EntityTypeBuilder<Measurement> builder)
    {
        builder.ToTable("measurements");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.ReceivedAt).HasDefaultValueSql("now()");

        builder.HasIndex(m => new { m.SeriesId, m.Timestamp });
        builder.HasIndex(m => m.Timestamp);
        builder.HasIndex(m => m.SensorId);

        builder.HasOne(m => m.Series)
            .WithMany(s => s.Measurements)
            .HasForeignKey(m => m.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Sensor)
            .WithMany(s => s.Measurements)
            .HasForeignKey(m => m.SensorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
