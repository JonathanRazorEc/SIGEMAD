using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ImpactoClasificadoConfiguration : IEntityTypeConfiguration<ImpactoClasificado>
{
    public void Configure(EntityTypeBuilder<ImpactoClasificado> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("ImpactoClasificado");

        builder.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(255);

        builder.HasOne(d => d.TipoImpacto).WithMany()
            .HasForeignKey(d => d.IdTipoImpacto)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.CategoriaImpacto).WithMany()
            .HasForeignKey(d => d.IdCategoriaImpacto)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.ClaseImpacto).WithMany()
            .HasForeignKey(d => d.IdClaseImpacto)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.TipoActuacion).WithMany()
            .HasForeignKey(d => d.IdTipoActuacion)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
