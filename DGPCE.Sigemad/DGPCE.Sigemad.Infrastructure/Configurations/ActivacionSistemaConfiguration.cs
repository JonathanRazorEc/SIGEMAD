using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ActivacionSistemaConfiguration : IEntityTypeConfiguration<ActivacionSistema>
{
    public void Configure(EntityTypeBuilder<ActivacionSistema> builder)
    {
        builder.ToTable(nameof(ActivacionSistema));

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Codigo)
            .HasMaxLength(30);

        builder.Property(a => a.Nombre)
            .HasMaxLength(300);


        // Configuración para `FechaAceptacion` con DateOnly
        builder.Property(d => d.FechaAceptacion)
            .HasConversion<DateOnlyConverter>()
            .IsRequired(false);

        // Configuración para `FechaActivacion` con DateOnly
        builder.Property(d => d.FechaActivacion)
            .HasConversion<DateOnlyConverter>()
            .IsRequired(false);


        builder.HasOne(a => a.ModoActivacion)
          .WithMany()
          .HasForeignKey(a => a.IdModoActivacion)
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired(false);

        builder.Property(a => a.Autoridad)
            .HasMaxLength(510)
            .IsRequired();


        builder.HasOne(d => d.Registro)
            .WithMany(e => e.ActivacionSistemas)
            .HasForeignKey(d => d.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TipoSistemaEmergencia)
         .WithMany()
         .HasForeignKey(a => a.IdTipoSistemaEmergencia)
         .OnDelete(DeleteBehavior.Restrict);
    }
}