using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
internal class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
{

    public void Configure(EntityTypeBuilder<Municipio> builder)
    {
        builder.ToTable(nameof(Municipio));

        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Provincia)
                           .WithMany()
                           .HasForeignKey(e => e.IdProvincia)
                           .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.CreadoPor)
              .HasMaxLength(500)
              .IsUnicode(false);

        builder.Property(e => e.FechaCreacion).HasColumnType("datetime");
        builder.Property(e => e.FechaModificacion).HasColumnType("datetime");
        builder.Property(e => e.FechaEliminacion).HasColumnType("datetime");
        builder.Property(e => e.ModificadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(e => e.EliminadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(e => e.GeoPosicion).HasColumnType("geometry");
    }
}
