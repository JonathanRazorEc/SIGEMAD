using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class PlanEmergenciaConfiguration : IEntityTypeConfiguration<PlanEmergencia>
{
    public void Configure(EntityTypeBuilder<PlanEmergencia> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("PlanEmergencia");

        builder.Property(e => e.IdCcaa).HasColumnName("IdCCAA");


        builder.HasOne(d => d.Provincia).WithMany()
            .HasForeignKey(d => d.IdProvincia)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Municipio).WithMany()
            .HasForeignKey(d => d.IdMunicipio)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(d => d.Ccaa).WithMany()
            .HasForeignKey(d => d.IdCcaa)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.TipoPlan).WithMany()
        .HasForeignKey(d => d.IdTipoPlan)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.TipoRiesgo).WithMany()
        .HasForeignKey(d => d.IdTipoRiesgo)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.AmbitoPlan).WithMany()
        .HasForeignKey(d => d.IdAmbitoPlan)
        .OnDelete(DeleteBehavior.Restrict);

    }
}
