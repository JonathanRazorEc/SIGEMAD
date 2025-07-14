using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

internal class IncendioConfiguration : IEntityTypeConfiguration<Incendio>
{
    public void Configure(EntityTypeBuilder<Incendio> builder)
    {
        builder.HasKey(e => e.Id).HasName("Sucesos_PK");

        builder.ToTable("Incendio");

        builder.HasIndex(e => e.Denominacion, "IX_Incendio");

        builder.HasIndex(e => e.IdSuceso, "IX_Incendio_1");

        builder.HasIndex(e => new { e.FechaCreacion, e.FechaModificacion }, "IX_Incendio_3");

        builder.Property(e => e.Id)
            .UseIdentityColumn();

        builder.Property(e => e.CreadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(e => e.Denominacion)
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(e => e.FechaCreacion).HasColumnType("datetime");
        builder.Property(e => e.FechaInicio).HasColumnType("datetime");
        builder.Property(e => e.FechaModificacion).HasColumnType("datetime");
        builder.Property(e => e.GeoPosicion).HasColumnType("geometry");
        builder.Property(e => e.RutaMapaRiesgo).HasColumnType("text");

        builder.Property(e => e.ModificadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(e => e.UtmX).HasColumnName("UTM_X");
        builder.Property(e => e.UtmY).HasColumnName("UTM_Y");

        builder.HasOne(d => d.ClaseSuceso).WithMany()
            .HasForeignKey(d => d.IdClaseSuceso)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("ClaseSucesoIncendio");

        builder.HasOne(d => d.Suceso)
            .WithMany(d => d.Incendios)
            .HasForeignKey(d => d.IdSuceso)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("SucesoIncendio");

        builder.HasOne(d => d.Territorio).WithMany()
            .HasForeignKey(d => d.IdTerritorio)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("IncendioTerritorio");

        builder.HasOne(d => d.EstadoSuceso).WithMany()
            .HasForeignKey(d => d.IdEstadoSuceso)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Incendio_EstadoSuceso");

        builder.HasOne(d => d.Pais).WithMany()
            .HasForeignKey(d => d.IdPais)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Distrito).WithMany()
            .HasForeignKey(d => d.IdDistrito)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.MunicipioExtranjero).WithMany()
            .HasForeignKey(d => d.IdMunicipioExtranjero)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Provincia).WithMany()
            .HasForeignKey(d => d.IdProvincia)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Municipio).WithMany()
            .HasForeignKey(d => d.IdMunicipio)
            .OnDelete(DeleteBehavior.Restrict);

    }
}