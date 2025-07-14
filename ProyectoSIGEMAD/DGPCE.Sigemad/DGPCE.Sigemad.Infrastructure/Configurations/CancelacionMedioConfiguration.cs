using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class CancelacionMedioConfiguration : IEntityTypeConfiguration<CancelacionMedio>
{
    public void Configure(EntityTypeBuilder<CancelacionMedio> builder)
    {
        builder.ToTable(nameof(CancelacionMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.EjecucionPaso)
            .WithOne(m => m.CancelacionMedio)
            .HasForeignKey<CancelacionMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
