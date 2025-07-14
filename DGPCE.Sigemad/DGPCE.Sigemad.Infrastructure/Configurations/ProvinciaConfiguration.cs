using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class ProvinciaConfiguration : IEntityTypeConfiguration<Provincia>
    {
        public void Configure(EntityTypeBuilder<Provincia> builder)
        {
            builder.HasKey(e => e.Id).HasName("Provincias_PK");

            builder.ToTable("Provincia");

            builder.HasMany(p => p.Municipios).WithOne(m => m.Provincia)
             .HasForeignKey(m => m.IdProvincia);

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
}
