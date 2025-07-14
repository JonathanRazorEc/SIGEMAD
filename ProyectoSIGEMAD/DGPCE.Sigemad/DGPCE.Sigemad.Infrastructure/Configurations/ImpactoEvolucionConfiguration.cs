using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ImpactoEvolucionConfiguration : IEntityTypeConfiguration<ImpactoEvolucion>
{
    public void Configure(EntityTypeBuilder<ImpactoEvolucion> builder)
    {
        builder.HasKey(e => e.Id);
        builder.ToTable("ImpactoEvolucion");

        builder.HasOne(d => d.TipoImpactoEvolucion)
            .WithMany(d => d.ImpactosEvoluciones)
            .HasForeignKey(d => d.IdTipoImpactoEvolucion)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.ImpactoClasificado)
            .WithMany()
            .HasForeignKey(d => d.IdImpactoClasificado)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.TipoDanio)
            .WithMany()
            .HasForeignKey(d => d.IdTipoDanio)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.AlteracionInterrupcion)
            .WithMany()
            .HasForeignKey(d => d.IdAlteracionInterrupcion)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.ZonaPlanificacion)
            .WithMany()
            .HasForeignKey(d => d.IdZonaPlanificacion)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Provincia).WithMany()
            .HasForeignKey(d => d.IdProvincia)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Municipio).WithMany()
            .HasForeignKey(d => d.IdMunicipio)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
