using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

class MunicipioExtranjeroConfiguration : IEntityTypeConfiguration<MunicipioExtranjero>
{
    public void Configure(EntityTypeBuilder<MunicipioExtranjero> builder)
    {
        builder.ToTable(nameof(MunicipioExtranjero));

        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Distrito)
                           .WithMany()
                           .HasForeignKey(e => e.IdDistrito)
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
