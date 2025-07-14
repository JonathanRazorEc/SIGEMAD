using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AuditoriaEmergenciaNacionalConfiguration : IEntityTypeConfiguration<AuditoriaEmergenciaNacional>
{
    public void Configure(EntityTypeBuilder<AuditoriaEmergenciaNacional> builder)
    {
        // Configura la tabla, columnas, etc.
        builder.ToTable("Auditoria_EmergenciaNacional");

        // Configuración de la relación con ActuacionRelevanteDGPCE:
        // Supongamos que en tu entidad AuditoriaEmergenciaNacional tienes una propiedad:
        // public int IdActuacionRelevanteDGPCE { get; set; }
        // y una navegación:
        // public virtual ActuacionRelevanteDGPCE ActuacionRelevanteDGPCE { get; set; }
        // Entonces:
        builder.HasOne(a => a.ActuacionRelevanteDGPCE)
               .WithOne(ar => ar.AuditoriaEmergenciaNacional)
               .HasForeignKey<AuditoriaEmergenciaNacional>(a => a.IdEmergencia)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_AuditoriaEmergenciaNacional_ActuacionRelevanteDGPCE");
    }
}
