using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class CoordinacionCecopiConfiguration : IEntityTypeConfiguration<CoordinacionCecopi>
{
    public void Configure(EntityTypeBuilder<CoordinacionCecopi> builder)
    {
        builder.ToTable(nameof(CoordinacionCecopi));

        builder.HasKey(c => c.Id);
        builder.Property(e => e.GeoPosicion).HasColumnType("geometry");
        builder.Property(e => e.IdRegistro).IsRequired();

        // Configuración para `FechaInicio` con DateOnly
        //builder.Property(d => d.FechaInicio)
        //    .HasConversion<DateOnlyConverter>()
        //    .IsRequired();

        //// Configuración para `FechaFin` con DateOnly
        //builder.Property(d => d.FechaFin)
        //    .HasConversion<DateOnlyConverter>()
        //    .IsRequired();

        builder.HasOne(c => c.Provincia)
           .WithMany()
           .HasForeignKey(d => d.IdProvincia)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Municipio)
           .WithMany()
           .HasForeignKey(d => d.IdMunicipio)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Registro)
            .WithMany(dce => dce.CoordinacionesCecopi)
            .HasForeignKey(d => d.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Archivo)
           .WithMany()
           .HasForeignKey(d => d.IdArchivo);


        //builder.HasOne(d => d.TipoGestionDireccion)
        //   .WithMany()
        //   .HasForeignKey(d => d.IdTipoGestionDireccion)
        //   .OnDelete(DeleteBehavior.Restrict);
    }
}
