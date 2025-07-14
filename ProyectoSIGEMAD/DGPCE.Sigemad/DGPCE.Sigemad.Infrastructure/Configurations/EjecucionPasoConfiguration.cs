using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class EjecucionPasoConfiguration : IEntityTypeConfiguration<EjecucionPaso>
{
    public void Configure(EntityTypeBuilder<EjecucionPaso> builder)
    {
        builder.ToTable(nameof(EjecucionPaso));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.MovilizacionMedio)
            .WithMany(m => m.Pasos)
            .HasForeignKey(builder => builder.IdMovilizacionMedio)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.PasoMovilizacion)
            .WithMany()
            .HasForeignKey(builder => builder.IdPasoMovilizacion)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
