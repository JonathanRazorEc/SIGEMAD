using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AuditoriaLlegadaBaseMedioConfiguration : IEntityTypeConfiguration<AuditoriaLlegadaBaseMedio>
{
    public void Configure(EntityTypeBuilder<AuditoriaLlegadaBaseMedio> builder)
    {
        builder.ToTable(nameof(AuditoriaLlegadaBaseMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.AuditoriaEjecucionPaso)
            .WithOne(m => m.AuditoriaLlegadaBaseMedio)
            .HasForeignKey<AuditoriaLlegadaBaseMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(builder => builder.Capacidad)
            .WithMany()
            .HasForeignKey(builder => builder.IdCapacidad)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
