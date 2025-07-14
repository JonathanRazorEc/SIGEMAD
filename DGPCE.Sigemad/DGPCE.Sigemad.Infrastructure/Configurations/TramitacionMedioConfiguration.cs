using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class TramitacionMedioConfiguration : IEntityTypeConfiguration<TramitacionMedio>
{
    public void Configure(EntityTypeBuilder<TramitacionMedio> builder)
    {
        builder.ToTable(nameof(TramitacionMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(x => x.EjecucionPaso)
            .WithOne(x => x.TramitacionMedio)
            .HasForeignKey<TramitacionMedio>(x => x.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(builder => builder.DestinoMedio)
            .WithMany()
            .HasForeignKey(builder => builder.IdDestinoMedio)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
