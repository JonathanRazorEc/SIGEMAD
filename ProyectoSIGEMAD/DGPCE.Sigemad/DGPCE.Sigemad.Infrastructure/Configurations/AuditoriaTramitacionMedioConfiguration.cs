using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AuditoriaTramitacionMedioConfiguration : IEntityTypeConfiguration<AuditoriaTramitacionMedio>
{
    public void Configure(EntityTypeBuilder<AuditoriaTramitacionMedio> builder)
    {
        builder.ToTable(nameof(AuditoriaTramitacionMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(x => x.AuditoriaEjecucionPaso)
            .WithOne(x => x.AuditoriaTramitacionMedio)
            .HasForeignKey<AuditoriaTramitacionMedio>(x => x.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(builder => builder.DestinoMedio)
            .WithMany()
            .HasForeignKey(builder => builder.IdDestinoMedio)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
