using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class DetalleIntervencionMedioConfiguration : IEntityTypeConfiguration<DetalleIntervencionMedio>
{
    public void Configure(EntityTypeBuilder<DetalleIntervencionMedio> builder)
    {
        builder.ToTable(nameof(DetalleIntervencionMedio));

        builder.HasKey(d => new { d.IdIntervencionMedio, d.IdMediosCapacidad });

        builder.HasOne(s => s.MediosCapacidad)
            .WithMany()
            .HasForeignKey(s => s.IdMediosCapacidad)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.IntervencionMedio)
            .WithMany(a => a.DetalleIntervencionMedios)
            .HasForeignKey(s => s.IdIntervencionMedio)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
