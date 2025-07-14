using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AuditoriaCancelacionMedioConfiguration : IEntityTypeConfiguration<AuditoriaCancelacionMedio>
{
    public void Configure(EntityTypeBuilder<AuditoriaCancelacionMedio> builder)
    {
        builder.ToTable(nameof(AuditoriaCancelacionMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.AuditoriaEjecucionPaso)
            .WithOne(m => m.AuditoriaCancelacionMedio)
            .HasForeignKey<AuditoriaCancelacionMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
