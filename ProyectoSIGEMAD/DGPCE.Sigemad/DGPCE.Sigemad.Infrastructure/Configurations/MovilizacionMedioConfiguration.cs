using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class MovilizacionMedioConfiguration : IEntityTypeConfiguration<MovilizacionMedio>
{
    public void Configure(EntityTypeBuilder<MovilizacionMedio> builder)
    {
        builder.ToTable(nameof(MovilizacionMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.ActuacionRelevanteDGPCE)
            .WithMany(a => a.MovilizacionMedios)
            .HasForeignKey(builder => builder.IdActuacionRelevanteDGPCE)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
