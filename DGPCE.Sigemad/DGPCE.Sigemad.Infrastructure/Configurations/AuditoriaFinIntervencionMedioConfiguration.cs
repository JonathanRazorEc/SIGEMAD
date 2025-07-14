using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AuditoriaFinIntervencionMedioConfiguration : IEntityTypeConfiguration<AuditoriaFinIntervencionMedio>
{
    public void Configure(EntityTypeBuilder<AuditoriaFinIntervencionMedio> builder)
    {
        builder.ToTable(nameof(AuditoriaFinIntervencionMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.AuditoriaEjecucionPaso)
            .WithOne(m => m.AuditoriaFinIntervencionMedio)
            .HasForeignKey<AuditoriaFinIntervencionMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(builder => builder.Capacidad)
            .WithMany()
            .HasForeignKey(builder => builder.IdCapacidad)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
