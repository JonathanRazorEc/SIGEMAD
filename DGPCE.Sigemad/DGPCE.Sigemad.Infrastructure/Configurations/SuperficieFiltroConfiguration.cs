using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class SuperficieFiltroConfiguration : IEntityTypeConfiguration<SuperficieFiltro>
{
    public void Configure(EntityTypeBuilder<SuperficieFiltro> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.TipoFiltro)
            .WithMany()
            .HasForeignKey(builder => builder.IdTipoFiltro)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
