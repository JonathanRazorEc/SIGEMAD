using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class OfrecimientoMedioConfiguration : IEntityTypeConfiguration<OfrecimientoMedio>
{
    public void Configure(EntityTypeBuilder<OfrecimientoMedio> builder)
    {
        builder.ToTable(nameof(OfrecimientoMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.EjecucionPaso)
            .WithOne(m => m.OfrecimientoMedio)
            .HasForeignKey<OfrecimientoMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
