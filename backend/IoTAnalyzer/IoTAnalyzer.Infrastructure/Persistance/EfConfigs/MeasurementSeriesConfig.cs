using IoTAnalyzer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTAnalyzer.Infrastructure.Persistance.EfConfigs;

public class MeasurementSeriesConfig : IEntityTypeConfiguration<MeasurementSeries>
{
    public void Configure(EntityTypeBuilder<MeasurementSeries> builder)
    {
        builder.ToTable("series", t =>
        {
            t.HasCheckConstraint("ck_series_min_lt_max", "min_value < max_value");
            t.HasCheckConstraint("ck_series_color_hex", "color ~ '^#[0-9a-fA-F]{6}$'");
            t.HasCheckConstraint("ck_series_name_not_blank", "length(btrim(name)) > 0");
        });
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
        builder.Property(s => s.Color).HasMaxLength(7).IsFixedLength().IsRequired();
        builder.Property(s => s.Icon).HasMaxLength(50);
        builder.Property(s => s.Unit).HasMaxLength(20);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("now()");
    }
}
