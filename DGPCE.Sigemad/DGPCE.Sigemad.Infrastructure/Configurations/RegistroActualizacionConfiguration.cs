using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class RegistroActualizacionConfiguration : IEntityTypeConfiguration<RegistroActualizacion>
{
    public void Configure(EntityTypeBuilder<RegistroActualizacion> builder)
    {
        builder.ToTable("RegistroActualizacion");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.TipoEntidad)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(r => r.TipoRegistroActualizacion)
            .WithMany(t => t.Registros)
            .HasForeignKey(r => r.IdTipoRegistroActualizacion);

        builder.Property(r => r.Borrado)
            .HasDefaultValue(false);
    }
}
