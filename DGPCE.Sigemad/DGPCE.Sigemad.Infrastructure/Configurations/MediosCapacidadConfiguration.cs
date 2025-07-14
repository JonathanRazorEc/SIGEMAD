using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class MediosCapacidadConfiguration : IEntityTypeConfiguration<MediosCapacidad>
{
    public void Configure(EntityTypeBuilder<MediosCapacidad> builder)
    {
        builder.ToTable("MediosCapacidad");

        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.TipoCapacidad)
            .WithMany()
            .HasForeignKey(d => d.IdTipoCapacidad)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.TipoMedio)
            .WithMany()
            .HasForeignKey(d => d.IdTipoMedio)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
