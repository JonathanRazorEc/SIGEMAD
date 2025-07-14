using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class DetalleRegistroActualizacionConfiguration : IEntityTypeConfiguration<DetalleRegistroActualizacion>
{
    public void Configure(EntityTypeBuilder<DetalleRegistroActualizacion> builder)
    {
        builder.ToTable("DetalleRegistroActualizacion");

        builder.HasKey(d => d.Id);

        // Esto guarda el `enum` como `int` en la BD
        builder.Property(d => d.IdEstadoRegistro)
            .HasConversion<int>().
            IsRequired();

        builder.HasOne(d => d.RegistroActualizacion)
            .WithMany(r => r.DetallesRegistro)
            .HasForeignKey(d => d.IdRegistroActualizacion);


        builder.HasOne(d => d.ApartadoRegistro)
            .WithMany(a => a.DetallesRegistro)
            .HasForeignKey(d => d.IdApartadoRegistro);
            //.OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.HistorialCambios)
            .WithOne(h => h.DetalleRegistroActualizacion)
            .HasForeignKey(h => h.IdDetalleRegistroActualizacion);
    }
}
