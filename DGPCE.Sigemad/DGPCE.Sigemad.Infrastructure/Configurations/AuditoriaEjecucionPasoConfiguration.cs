using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AuditoriaEjecucionPasoConfiguration : IEntityTypeConfiguration<AuditoriaEjecucionPaso>
{
    public void Configure(EntityTypeBuilder<AuditoriaEjecucionPaso> builder)
    {
        builder.ToTable(nameof(AuditoriaEjecucionPaso));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.AuditoriaMovilizacionMedio)
            .WithMany(m => m.Pasos)
            .HasForeignKey(builder => builder.IdMovilizacionMedio)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.PasoMovilizacion)
            .WithMany()
            .HasForeignKey(builder => builder.IdPasoMovilizacion)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
