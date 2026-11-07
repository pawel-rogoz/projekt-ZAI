using IoTAnalyzer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTAnalyzer.Infrastructure.Persistance.EfConfigs;

public class AuthSessionConfig : IEntityTypeConfiguration<AuthSession>
{
    public void Configure(EntityTypeBuilder<AuthSession> builder)
    {
        builder.ToTable("auth_sessions");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TokenHash).HasMaxLength(64).IsFixedLength().IsRequired();
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("now()");

        builder.HasIndex(s => s.TokenHash).IsUnique();
        builder.HasIndex(s => s.ExpiresAt);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
