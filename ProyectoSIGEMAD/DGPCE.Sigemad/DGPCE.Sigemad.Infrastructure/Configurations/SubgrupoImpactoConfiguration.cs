using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class SubgrupoImpactoConfiguration : IEntityTypeConfiguration<SubgrupoImpacto>
{
    public void Configure(EntityTypeBuilder<SubgrupoImpacto> builder)
    {
        builder.ToTable(nameof(SubgrupoImpacto));

        builder.Property(e => e.Id)
        .UseIdentityColumn();

        builder.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(100);

        builder.HasOne(d => d.GrIconoImpacto).WithMany()
            .HasForeignKey(d => d.IdGrIconoImpacto)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
