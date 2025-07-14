using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class TipoRegistroActualizacionConfiguration : IEntityTypeConfiguration<TipoRegistroActualizacion>
{
    public void Configure(EntityTypeBuilder<TipoRegistroActualizacion> builder)
    {
        builder.ToTable("TipoRegistroActualizacion");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Nombre)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(t => t.Apartados)
            .WithOne(a => a.TipoRegistroActualizacion)
            .HasForeignKey(a => a.IdTipoRegistroActualizacion);

        builder.HasMany(t => t.Registros)
            .WithOne(r => r.TipoRegistroActualizacion)
            .HasForeignKey(r => r.IdTipoRegistroActualizacion);
    }
}
