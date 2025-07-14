using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class CapacidadConfiguration : IEntityTypeConfiguration<Capacidad>
{
    public void Configure(EntityTypeBuilder<Capacidad> builder)
    {
        builder.ToTable(nameof(Capacidad));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.TipoCapacidad)
            .WithMany()
            .HasForeignKey(builder => builder.IdTipoCapacidad)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.Entidad)
            .WithMany()
            .HasForeignKey(builder => builder.IdEntidad)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
