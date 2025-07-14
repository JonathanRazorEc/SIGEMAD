using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    internal class DistritoConfiguration : IEntityTypeConfiguration<Distrito>
    {

       

        public void Configure(EntityTypeBuilder<Distrito> builder)
        {
            builder.HasKey(e => e.Id).HasName("Distrito_PK");

            builder.ToTable("Distrito");

            builder.Property(e => e.IdPais)
                .IsRequired();

            builder.HasOne(e => e.Pais)
                               .WithMany()
                               .HasForeignKey(e => e.IdPais)
                               .OnDelete(DeleteBehavior.Restrict)
                               .HasConstraintName("PaisDistrito");

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
        }
    }
}
