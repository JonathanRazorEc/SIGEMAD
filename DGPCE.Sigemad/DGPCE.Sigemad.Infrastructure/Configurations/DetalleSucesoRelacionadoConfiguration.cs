using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class DetalleSucesoRelacionadoConfiguration : IEntityTypeConfiguration<DetalleSucesoRelacionado>
{
    public void Configure(EntityTypeBuilder<DetalleSucesoRelacionado> builder)
    {
        builder.ToTable("DetalleSucesoRelacionado");

        // Definir la clave compuesta
        builder.HasKey(ds => new { ds.IdCabeceraSuceso, ds.IdSucesoAsociado });

        // Relaciones
        builder.HasOne(ds => ds.SucesoRelacionado)
            .WithMany(sr => sr.DetalleSucesoRelacionados)
            .HasForeignKey(ds => ds.IdCabeceraSuceso)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ds => ds.SucesoAsociado)
            .WithMany()
            .HasForeignKey(ds => ds.IdSucesoAsociado)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
