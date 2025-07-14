using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
internal class DetalleOtraInformacionConfiguration : IEntityTypeConfiguration<DetalleOtraInformacion>
{
    public void Configure(EntityTypeBuilder<DetalleOtraInformacion> builder)
    {
        builder.HasKey(e => e.Id).HasName("DetalleOtraInformacion_PK");

        builder.ToTable("DetalleOtraInformacion");

        builder.Property(e => e.Id)
            .UseIdentityColumn();

        builder.Property(e => e.FechaHora)
            .IsRequired();

        builder.Property(e => e.IdMedio)
            .IsRequired();

        builder.Property(e => e.Asunto)
            .HasColumnType("nvarchar(500)")
            .IsRequired(false);

        builder.Property(e => e.Observaciones)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);

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

        
        builder.HasMany(d => d.ProcedenciasDestinos)
            .WithOne(p => p.DetalleOtraInformacion)
            .HasForeignKey(p => p.IdDetalleOtraInformacion)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_DetalleOtraInformacion_ProcedenciaDestino");
        
        builder.HasOne(d => d.Medio)
            .WithMany()
            .HasForeignKey(d => d.IdMedio)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_DetalleOtraInformacion_Medio");
    }
}
