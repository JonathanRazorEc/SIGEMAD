using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class SucesoConfiguration : IEntityTypeConfiguration<Suceso>
{
    public void Configure(EntityTypeBuilder<Suceso> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("Suceso");

        builder.Property(e => e.IdTipo).HasDefaultValue(1);

        builder.HasOne(d => d.TipoSuceso).WithMany()
            .HasForeignKey(d => d.IdTipo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.RegistroActualizaciones)
            .WithOne(e => e.Suceso)
            .HasForeignKey(e => e.IdSuceso)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
