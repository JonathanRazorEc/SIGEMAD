using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class AuditoriaDeclaracionZAGEPConfiguration : IEntityTypeConfiguration<AuditoriaDeclaracionZAGEP>
{
    public void Configure(EntityTypeBuilder<AuditoriaDeclaracionZAGEP> builder)
    {
        builder.ToTable("Auditoria_DeclaracionZAGEP");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.IdActuacionRelevanteDGPCE).IsRequired();

        builder.HasOne(e => e.ActuacionRelevanteDGPCE)
               .WithMany(a => a.AuditoriaDeclaracionesZAGEP)
               .HasForeignKey(e => e.IdActuacionRelevanteDGPCE)
               .HasConstraintName("FK_Auditoria_DeclaracionZAGEP_ActuacionRelevanteDGPCE");
    }
}
