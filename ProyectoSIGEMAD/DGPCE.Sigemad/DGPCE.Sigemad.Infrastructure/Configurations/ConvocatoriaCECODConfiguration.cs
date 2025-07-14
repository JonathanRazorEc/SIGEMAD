using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DGPCE.Sigemad.Infrastructure.Converters;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ConvocatoriaCECODConfiguration : IEntityTypeConfiguration<ConvocatoriaCECOD>
{

    public void Configure(EntityTypeBuilder<ConvocatoriaCECOD> builder)
    {
        builder.ToTable(nameof(ConvocatoriaCECOD));
        builder.HasKey(c => c.Id);
        builder.Property(e => e.IdActuacionRelevanteDGPCE)
             .IsRequired();

        builder.Property(e => e.Lugar)
                .IsRequired()
               .HasMaxLength(510);

         builder.Property(e => e.Convocados)
            .IsRequired()
           .HasMaxLength(510);

        builder.Property(e => e.Participantes)
             .IsRequired()
            .HasMaxLength(510);

        // Configuración para `FechaInicio` con DateOnly
        builder.Property(d => d.FechaInicio)
            .HasConversion<DateOnlyConverter>()
            .IsRequired();

        // Configuración para `FechaFin` con DateOnly
        builder.Property(d => d.FechaFin)
            .HasConversion<DateOnlyConverter>()
            .IsRequired();


        builder.HasOne(d => d.ActuacionRelevanteDGPCE)
            .WithMany(dce => dce.ConvocatoriasCECOD)
            .HasForeignKey(d => d.IdActuacionRelevanteDGPCE)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
