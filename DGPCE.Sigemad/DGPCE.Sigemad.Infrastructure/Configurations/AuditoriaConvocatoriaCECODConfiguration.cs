using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class AuditoriaConvocatoriaCECODConfiguration : IEntityTypeConfiguration<AuditoriaConvocatoriaCECOD>
{
    public void Configure(EntityTypeBuilder<AuditoriaConvocatoriaCECOD> builder)
    {
        builder.ToTable("Auditoria_ConvocatoriaCECOD");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.IdActuacionRelevanteDGPCE).IsRequired();

        builder.HasOne(e => e.ActuacionRelevanteDGPCE)
               .WithMany(a => a.AuditoriaConvocatoriasCECOD)
               .HasForeignKey(e => e.IdActuacionRelevanteDGPCE)
               .HasConstraintName("FK_Auditoria_ConvocatoriaCECOD_ActuacionRelevanteDGPCE");
    }
}
