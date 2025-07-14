using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class EvolucionConfiguration : IEntityTypeConfiguration<Evolucion>
{
    public void Configure(EntityTypeBuilder<Evolucion> builder)
    {

        builder.ToTable("Evolucion");

        builder.Property(e => e.IdSuceso)
         .IsRequired();

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FechaCreacion)
            .HasColumnType("datetime");

        builder.Property(e => e.FechaModificacion)
            .HasColumnType("datetime");

        builder.Property(e => e.FechaModificacion)
            .HasColumnType("datetime");

        builder.Property(e => e.CreadoPor)
          .HasMaxLength(500)
          .IsUnicode(false);

        builder.Property(e => e.ModificadoPor)
          .HasMaxLength(500)
          .IsUnicode(false);

        builder.Property(e => e.EliminadoPor)
        .HasMaxLength(500)
        .IsUnicode(false);


        //// Relación uno a uno con Suceso
        //builder.HasOne(d => d.Suceso)
        //    .WithMany(s => s.Evoluciones)
        //    .HasForeignKey(d => d.IdSuceso)
        //    .OnDelete(DeleteBehavior.Restrict); // Evita eliminación en cascada

        // Índice Único: Solo una Evolución con `EsFoto = 0` por `IdSuceso`
        builder.HasIndex(e => new { e.IdSuceso, e.EsFoto })
            .IsUnique()
            .HasFilter("EsFoto = 0");
    }
}


