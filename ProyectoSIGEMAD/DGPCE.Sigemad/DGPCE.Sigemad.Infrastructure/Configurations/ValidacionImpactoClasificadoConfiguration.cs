using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ValidacionImpactoClasificadoConfiguration : IEntityTypeConfiguration<ValidacionImpactoClasificado>
{
    public void Configure(EntityTypeBuilder<ValidacionImpactoClasificado> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne(d => d.ImpactoClasificado)
            .WithMany()
            .HasForeignKey(d => d.IdImpactoClasificado)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
