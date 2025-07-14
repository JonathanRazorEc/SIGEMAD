using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class TipoImpactoEvolucionConfiguration : IEntityTypeConfiguration<TipoImpactoEvolucion>
{
    public void Configure(EntityTypeBuilder<TipoImpactoEvolucion> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("TipoImpactoEvolucion");

        builder.HasOne(d => d.Registro)
            .WithMany(e => e.TipoImpactosEvoluciones)
            .HasForeignKey(d => d.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.TipoImpacto).WithMany()
            .HasForeignKey(d => d.IdTipoImpacto)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
