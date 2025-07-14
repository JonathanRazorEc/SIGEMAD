using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class HistorialCambiosConfiguration : IEntityTypeConfiguration<HistorialCambios>
{
    public void Configure(EntityTypeBuilder<HistorialCambios> builder)
    {
        builder.ToTable("HistorialCambios");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.TablaModificada)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(h => h.CampoModificado)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(h => h.ValorAnterior)
            .HasMaxLength(500);

        builder.Property(h => h.ValorNuevo)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(h => h.DetalleRegistroActualizacion)
            .WithMany(d => d.HistorialCambios)
            .HasForeignKey(h => h.IdDetalleRegistroActualizacion);
    }
}
