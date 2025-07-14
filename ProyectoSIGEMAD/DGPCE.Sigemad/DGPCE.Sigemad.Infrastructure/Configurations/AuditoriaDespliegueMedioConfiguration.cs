using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AuditoriaDespliegueMedioConfiguration : IEntityTypeConfiguration<AuditoriaDespliegueMedio>
{
    public void Configure(EntityTypeBuilder<AuditoriaDespliegueMedio> builder)
    {
        builder.ToTable(nameof(AuditoriaDespliegueMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.AuditoriaEjecucionPaso)
            .WithOne(m => m.AuditoriaDespliegueMedio)
            .HasForeignKey<AuditoriaDespliegueMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(builder => builder.Capacidad)
            .WithMany()
            .HasForeignKey(builder => builder.IdCapacidad)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
