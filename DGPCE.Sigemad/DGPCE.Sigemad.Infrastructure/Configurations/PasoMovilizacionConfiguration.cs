using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class PasoMovilizacionConfiguration : IEntityTypeConfiguration<PasoMovilizacion>
{
    public void Configure(EntityTypeBuilder<PasoMovilizacion> builder)
    {
        builder.ToTable(nameof(PasoMovilizacion));
        builder.HasKey(c => c.Id);

        builder.HasOne(p => p.EstadoMovilizacion)
            .WithMany()
            .HasForeignKey(p => p.IdEstadoMovilizacion)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
