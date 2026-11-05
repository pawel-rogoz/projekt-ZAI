using IoTAnalyzer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTAnalyzer.Infrastructure.Persistance.EfConfigs;

public class SensorConfig : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.ToTable("sensors", t =>
            t.HasCheckConstraint("ck_sensors_name_not_blank", "length(btrim(name)) > 0"));
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
        builder.Property(s => s.ApiKeyHash).HasMaxLength(64).IsFixedLength().IsRequired();
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("now()");
        builder.Ignore(s => s.IsActive);

        builder.HasIndex(s => s.ApiKeyHash).IsUnique();

        builder.HasOne<MeasurementSeries>()
            .WithMany()
            .HasForeignKey(s => s.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
