using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class FlujoPasoMovilizacionConfiguration : IEntityTypeConfiguration<FlujoPasoMovilizacion>
{
    public void Configure(EntityTypeBuilder<FlujoPasoMovilizacion> builder)
    {
        builder.ToTable(nameof(FlujoPasoMovilizacion));
        builder.HasKey(c => c.Id);

        builder.HasOne(s => s.PasoActual)
            .WithMany()
            .HasForeignKey(s => s.IdPasoActual)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.PasoSiguiente)
            .WithMany()
            .HasForeignKey(s => s.IdPasoSiguiente)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
