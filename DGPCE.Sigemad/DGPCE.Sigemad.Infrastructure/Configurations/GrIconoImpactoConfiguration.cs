using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class GrIconoImpactoConfiguration : IEntityTypeConfiguration<GrIconoImpacto>
    {
        public void Configure(EntityTypeBuilder<GrIconoImpacto> builder)
        {
            builder.ToTable(nameof(GrIconoImpacto));

            builder.Property(e => e.Id)
            .UseIdentityColumn();

            builder.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(e => e.Imagen)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.HasOne(d => d.GrupoImpacto).WithMany()
                .HasForeignKey(d => d.IdGrupoImpacto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
