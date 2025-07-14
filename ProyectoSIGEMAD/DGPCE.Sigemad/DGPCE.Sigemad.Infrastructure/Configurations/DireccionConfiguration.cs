using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class DireccionConfiguration : IEntityTypeConfiguration<Direccion>
{
    public void Configure(EntityTypeBuilder<Direccion> builder)
    {
        builder.ToTable("Direccion");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.IdRegistro).IsRequired();

        // Configuración para `FechaInicio` con DateOnly
        //builder.Property(d => d.FechaInicio)
        //    .HasConversion<DateOnlyConverter>()
        //    .IsRequired();

        //// Configuración para `FechaFin` con DateOnly
        //builder.Property(d => d.FechaFin)
        //    .HasConversion<DateOnlyConverter>()
        //    .IsRequired();

        builder.HasOne(d => d.Registro)
            .WithMany(dce => dce.Direcciones)
            .HasForeignKey(d => d.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.TipoDireccionEmergencia)
           .WithMany()
           .HasForeignKey(d => d.IdTipoDireccionEmergencia)
           .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(d => d.TipoDireccionEmergencia)
           .WithMany()
           .HasForeignKey(d => d.IdTipoDireccionEmergencia)
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
