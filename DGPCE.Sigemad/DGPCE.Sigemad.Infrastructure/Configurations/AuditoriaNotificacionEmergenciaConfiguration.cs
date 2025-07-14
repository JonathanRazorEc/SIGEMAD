using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class AuditoriaNotificacionEmergenciaConfiguration : IEntityTypeConfiguration<AuditoriaNotificacionEmergencia>
{
    public void Configure(EntityTypeBuilder<AuditoriaNotificacionEmergencia> builder)
    {
        builder.ToTable("Auditoria_NotificacionEmergencia");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.IdActuacionRelevanteDGPCE).IsRequired();

        builder.HasOne(e => e.ActuacionRelevanteDGPCE)
               .WithMany(a => a.AuditoriaNotificacionesEmergencias)
               .HasForeignKey(e => e.IdActuacionRelevanteDGPCE)
               .HasConstraintName("FK_Auditoria_NotificacionEmergencia_ActuacionRelevanteDGPCE");

        builder.HasOne(a => a.TipoNotificacion)
           .WithMany()
           .HasForeignKey(a => a.IdTipoNotificacion)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
