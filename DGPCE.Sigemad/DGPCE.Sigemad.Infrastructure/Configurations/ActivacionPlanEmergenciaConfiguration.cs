using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ActivacionPlanEmergenciaConfiguration : IEntityTypeConfiguration<ActivacionPlanEmergencia>
{
    public void Configure(EntityTypeBuilder<ActivacionPlanEmergencia> builder)
    {
        builder.ToTable(nameof(ActivacionPlanEmergencia));

        builder.HasKey(a => a.Id);

        // Configuración para `FechaInicio` con DateOnly
        builder.Property(d => d.FechaHoraInicio)
            .IsRequired();

        // Configuración para `FechaFin` con DateOnly
        builder.Property(d => d.FechaHoraFin)
            .IsRequired(false);

        builder.Property(a => a.TipoPlanPersonalizado)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(a => a.PlanEmergenciaPersonalizado)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(a => a.Autoridad)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(a => a.Observaciones)
            .IsRequired(false);

        builder.HasOne(a => a.TipoPlan)
            .WithMany()
            .HasForeignKey(a => a.IdTipoPlan)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(a => a.PlanEmergencia)
            .WithMany()
            .HasForeignKey(a => a.IdPlanEmergencia)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(a => a.Archivo)
            .WithMany()
            .HasForeignKey(a => a.IdArchivo)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(e => e.Registro)
            .WithMany(e => e.ActivacionPlanEmergencias)
            .HasForeignKey(e => e.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
