using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ApartadoRegistroConfiguration : IEntityTypeConfiguration<ApartadoRegistro>
{
    public void Configure(EntityTypeBuilder<ApartadoRegistro> builder)
    {
        builder.ToTable("ApartadoRegistro");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nombre)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(a => a.TipoRegistroActualizacion)
            .WithMany(t => t.Apartados)
            .HasForeignKey(a => a.IdTipoRegistroActualizacion);

        builder.HasMany(a => a.DetallesRegistro)
            .WithOne(d => d.ApartadoRegistro)
            .HasForeignKey(d => d.IdApartadoRegistro)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.RegistrosApartados)
            .WithOne(ra => ra.ApartadoRegistro)
            .HasForeignKey(ra => ra.IdApartadoRegistro);
    }
}

