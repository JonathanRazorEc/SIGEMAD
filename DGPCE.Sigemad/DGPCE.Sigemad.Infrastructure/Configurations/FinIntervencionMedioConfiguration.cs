using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class FinIntervencionMedioConfiguration : IEntityTypeConfiguration<FinIntervencionMedio>
{
    public void Configure(EntityTypeBuilder<FinIntervencionMedio> builder)
    {
        builder.ToTable(nameof(FinIntervencionMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.EjecucionPaso)
            .WithOne(m => m.FinIntervencionMedio)
            .HasForeignKey<FinIntervencionMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(builder => builder.Capacidad)
            .WithMany()
            .HasForeignKey(builder => builder.IdCapacidad)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
