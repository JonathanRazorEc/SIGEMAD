using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AdministracionConfiguration : IEntityTypeConfiguration<Administracion>
{
    public void Configure(EntityTypeBuilder<Administracion> builder)
    {
        builder.ToTable(nameof(Administracion));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.TipoAdministracion)
            .WithMany()
            .HasForeignKey(builder => builder.IdTipoAdministracion)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

    }
}
