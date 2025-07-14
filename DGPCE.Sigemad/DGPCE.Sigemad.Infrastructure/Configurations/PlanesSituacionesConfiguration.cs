using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class PlanesSituacionesConfiguration : IEntityTypeConfiguration<PlanSituacion>
{
    public void Configure(EntityTypeBuilder<PlanSituacion> builder)
    {
        builder.ToTable("PlanSituacion");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Situacion)
           .HasMaxLength(300)
           .IsUnicode(false);

        builder.Property(e => e.SituacionEquivalente)
           .HasMaxLength(300)
           .IsUnicode(false);

        builder.Property(e => e.Nivel)
           .HasMaxLength(300)
           .IsUnicode(false);

        builder.HasOne(d => d.PlanEmergencia).WithMany()
                .HasForeignKey(d => d.IdPlanEmergencia)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.FaseEmergencia).WithMany()
                .HasForeignKey(d => d.IdFaseEmergencia)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
