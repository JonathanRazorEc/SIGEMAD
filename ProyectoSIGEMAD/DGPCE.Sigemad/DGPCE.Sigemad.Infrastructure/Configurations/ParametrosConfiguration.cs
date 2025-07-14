using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class ParametrosConfiguration : IEntityTypeConfiguration<Parametro>
{
    public void Configure(EntityTypeBuilder<Parametro> builder)
    {

        builder.ToTable("Parametro");
        builder.HasQueryFilter(r => r.Borrado == false);

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

        builder.HasOne(d => d.Registro)
            .WithMany(e => e.Parametros)
            .HasForeignKey(d => d.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.EstadoIncendio)
            .WithMany()
            .HasForeignKey(d => d.IdEstadoIncendio)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.PlanEmergencia)
            .WithMany()
            .HasForeignKey(d => d.IdPlanEmergencia)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.FaseEmergencia)
            .WithMany()
            .HasForeignKey(d => d.IdFaseEmergencia)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.PlanSituacion)
            .WithMany()
            .HasForeignKey(d => d.IdPlanSituacion)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.SituacionEquivalente)
            .WithMany()
            .HasForeignKey(d => d.IdSituacionEquivalente)
            .OnDelete(DeleteBehavior.Restrict);
    }
}