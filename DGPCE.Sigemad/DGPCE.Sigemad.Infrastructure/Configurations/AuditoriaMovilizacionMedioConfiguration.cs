using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class AuditoriaMovilizacionMedioConfiguration : IEntityTypeConfiguration<AuditoriaMovilizacionMedio>
{
    public void Configure(EntityTypeBuilder<AuditoriaMovilizacionMedio> builder)
    {
        builder.ToTable("Auditoria_MovilizacionMedio");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.IdActuacionRelevanteDGPCE).IsRequired();

        builder.HasOne(e => e.ActuacionRelevanteDGPCE)
               .WithMany(a => a.AuditoriaMovilizacionMedios)
               .HasForeignKey(e => e.IdActuacionRelevanteDGPCE)
               .HasConstraintName("FK_Auditoria_MovilizacionMedio_ActuacionRelevanteDGPCE");
    }
}
