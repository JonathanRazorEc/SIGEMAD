using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class DespliegueMedioConfiguration : IEntityTypeConfiguration<DespliegueMedio>
{
    public void Configure(EntityTypeBuilder<DespliegueMedio> builder)
    {
        builder.ToTable(nameof(DespliegueMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.EjecucionPaso)
            .WithOne(m => m.DespliegueMedio)
            .HasForeignKey<DespliegueMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(builder => builder.Capacidad)
            .WithMany()
            .HasForeignKey(builder => builder.IdCapacidad)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
