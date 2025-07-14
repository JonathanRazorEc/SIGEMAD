using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class LlegadaBaseMedioConfiguration : IEntityTypeConfiguration<LlegadaBaseMedio>
{
    public void Configure(EntityTypeBuilder<LlegadaBaseMedio> builder)
    {
        builder.ToTable(nameof(LlegadaBaseMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.EjecucionPaso)
            .WithOne(m => m.LlegadaBaseMedio)
            .HasForeignKey<LlegadaBaseMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(builder => builder.Capacidad)
            .WithMany()
            .HasForeignKey(builder => builder.IdCapacidad)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
