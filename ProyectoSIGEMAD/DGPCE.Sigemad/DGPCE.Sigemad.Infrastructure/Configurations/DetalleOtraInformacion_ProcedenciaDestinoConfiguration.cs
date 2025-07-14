using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
internal class DetalleOtraInformacion_ProcedenciaDestinoConfiguration : IEntityTypeConfiguration<DetalleOtraInformacion_ProcedenciaDestino>
{
    public void Configure(EntityTypeBuilder<DetalleOtraInformacion_ProcedenciaDestino> builder)
    {
        builder.ToTable("DetalleOtraInformacion_ProcedenciaDestino");
        builder.HasKey(e => new { e.IdProcedenciaDestino, e.IdDetalleOtraInformacion });
        builder.Property(e => e.IdDetalleOtraInformacion)
            .IsRequired();
        builder.Property(e => e.IdProcedenciaDestino)
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

        builder.HasOne(d => d.DetalleOtraInformacion)
            .WithMany(d => d.ProcedenciasDestinos)
            .HasForeignKey(d => d.IdDetalleOtraInformacion)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_DetalleOtraInformacion_ProcedenciaDestino_DetalleOtraInformacion");
        builder.HasOne(d => d.ProcedenciaDestino)
            .WithMany()
            .HasForeignKey(d => d.IdProcedenciaDestino)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_DetalleOtraInformacion_ProcedenciaDestino_ProcedenciaDestino");
    }
}

