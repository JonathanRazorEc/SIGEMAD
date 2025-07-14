using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AuditoriaOfrecimientoMedioConfiguration : IEntityTypeConfiguration<AuditoriaOfrecimientoMedio>
{
    public void Configure(EntityTypeBuilder<AuditoriaOfrecimientoMedio> builder)
    {
        builder.ToTable(nameof(AuditoriaOfrecimientoMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.AuditoriaEjecucionPaso)
            .WithOne(m => m.AuditoriaOfrecimientoMedio)
            .HasForeignKey<AuditoriaOfrecimientoMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
