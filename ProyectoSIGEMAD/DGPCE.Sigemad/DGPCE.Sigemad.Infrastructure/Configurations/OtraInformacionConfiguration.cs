using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
internal class OtraInformacionConfiguration :IEntityTypeConfiguration<OtraInformacion>
{
    public void Configure(EntityTypeBuilder<OtraInformacion> builder)
    {
        builder.HasKey(e => e.Id).HasName("OtraInformacion_PK");

        builder.ToTable("OtraInformacion");

        builder.Property(e => e.Id)
            .UseIdentityColumn();

        builder.Property(e => e.IdSuceso)
            .IsRequired();

        builder.Property(e => e.FechaCreacion).HasColumnType("datetime");
        builder.Property(e => e.CreadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(e => e.FechaModificacion).HasColumnType("datetime");
        builder.Property(e => e.ModificadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(e => e.FechaEliminacion).HasColumnType("datetime");
        builder.Property(e => e.EliminadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(e => e.Borrado)
            .HasDefaultValue(false);
                        
        builder.HasMany(e => e.DetallesOtraInformacion)
            .WithOne(d => d.OtraInformacion)
            .HasForeignKey(d => d.IdOtraInformacion)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_DetalleOtraInformacion_OtraInformacion");

        builder.HasOne(e => e.Suceso)
        .WithMany(i => i.OtraInformaciones)
        .HasForeignKey(e => e.IdSuceso)
        .OnDelete(DeleteBehavior.Restrict)
        .HasConstraintName("FK_OtraInformacion_Incendio");
    }
}
