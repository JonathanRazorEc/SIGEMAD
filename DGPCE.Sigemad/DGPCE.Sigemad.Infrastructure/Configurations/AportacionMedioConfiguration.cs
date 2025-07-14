using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AportacionMedioConfiguration : IEntityTypeConfiguration<AportacionMedio>
{
    public void Configure(EntityTypeBuilder<AportacionMedio> builder)
    {
        builder.ToTable(nameof(AportacionMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.Capacidad)
            .WithMany()
            .HasForeignKey(builder => builder.IdCapacidad)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.TipoAdministracion)
            .WithMany()
            .HasForeignKey(builder => builder.IdTipoAdministracion)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.EjecucionPaso)
            .WithOne(m => m.AportacionMedio)
            .HasForeignKey<AportacionMedio>(builder => builder.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
