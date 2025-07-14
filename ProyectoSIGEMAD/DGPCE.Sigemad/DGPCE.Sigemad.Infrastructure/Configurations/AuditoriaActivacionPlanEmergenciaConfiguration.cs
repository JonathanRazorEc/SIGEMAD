using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class AuditoriaActivacionPlanEmergenciaConfiguration : IEntityTypeConfiguration<AuditoriaActivacionPlanEmergencia>
{
    public void Configure(EntityTypeBuilder<AuditoriaActivacionPlanEmergencia> builder)
    {
        builder.ToTable("Auditoria_ActivacionPlanEmergencia");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.IdActuacionRelevanteDGPCE).IsRequired();

        builder.HasOne(e => e.ActuacionRelevanteDGPCE)
               .WithMany(a => a.AuditoriaActivacionPlanEmergencias)
               .HasForeignKey(e => e.IdActuacionRelevanteDGPCE)
               .HasConstraintName("FK_Auditoria_ActivacionPlanEmergencia_ActuacionRelevanteDGPCE");

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
    }
}
