using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class AuditoriaActivacionSistemaConfiguration : IEntityTypeConfiguration<AuditoriaActivacionSistema>
{
    public void Configure(EntityTypeBuilder<AuditoriaActivacionSistema> builder)
    {
        builder.ToTable("Auditoria_ActivacionSistema");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.IdActuacionRelevanteDGPCE).IsRequired();

        builder.HasOne(e => e.ActuacionRelevanteDGPCE)
               .WithMany(a => a.AuditoriaActivacionSistemas)
               .HasForeignKey(e => e.IdActuacionRelevanteDGPCE)
               .HasConstraintName("FK_Auditoria_ActivacionSistema_ActuacionRelevanteDGPCE");

        builder.HasOne(a => a.TipoSistemaEmergencia)
    .WithMany()
    .HasForeignKey(a => a.IdTipoSistemaEmergencia)
    .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.ModoActivacion)
            .WithMany()
            .HasForeignKey(a => a.IdModoActivacion)
            .OnDelete(DeleteBehavior.Restrict);


    }
}