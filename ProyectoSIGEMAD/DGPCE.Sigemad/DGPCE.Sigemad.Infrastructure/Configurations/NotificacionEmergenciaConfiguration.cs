using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class NotificacionEmergenciaConfiguration : IEntityTypeConfiguration<NotificacionEmergencia>
{
    public void Configure(EntityTypeBuilder<NotificacionEmergencia> builder)
    {
        builder.ToTable(nameof(NotificacionEmergencia));

        builder.HasKey(a => a.Id);

        builder.Property(a => a.FechaHoraNotificacion)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(a => a.OrganosNotificados)
            .HasMaxLength(510)
            .IsRequired();

        builder.Property(a => a.OrganosNotificados)
            .HasMaxLength(510)
            .IsRequired(); 
        
        builder.Property(a => a.UCPM)
            .HasMaxLength(510);

        builder.Property(a => a.OrganismoInternacional)
            .HasMaxLength(510);

        builder.Property(a => a.OtrosPaises)
            .HasMaxLength(510);
     
        builder.HasOne(e => e.ActuacionRelevanteDGPCE)
          .WithMany(e => e.NotificacionesEmergencias)
          .HasForeignKey(e => e.IdActuacionRelevanteDGPCE)
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TipoNotificacion)
           .WithMany()
           .HasForeignKey(a => a.IdTipoNotificacion)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
