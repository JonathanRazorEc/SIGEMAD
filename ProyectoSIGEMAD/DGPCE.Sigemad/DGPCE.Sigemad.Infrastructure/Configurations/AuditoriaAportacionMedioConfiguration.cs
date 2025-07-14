using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AuditoriaAportacionMedioConfiguration : IEntityTypeConfiguration<AuditoriaAportacionMedio>
{
    public void Configure(EntityTypeBuilder<AuditoriaAportacionMedio> builder)
    {
        builder.ToTable(nameof(AuditoriaAportacionMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.Capacidad)
            .WithMany()
            .HasForeignKey(builder => builder.IdCapacidad)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.TipoAdministracion)
            .WithMany()
            .HasForeignKey(builder => builder.IdTipoAdministracion)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.AuditoriaEjecucionPaso)
            .WithOne(m => m.AuditoriaAportacionMedio)
            .HasForeignKey<AuditoriaAportacionMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
