using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class SubgrupoMedioConfiguration : IEntityTypeConfiguration<SubgrupoMedio>
{
    public void Configure(EntityTypeBuilder<SubgrupoMedio> builder)
    {
        builder.ToTable("SubgrupoMedio");
        builder.HasKey(s => s.Id);

        builder.HasOne(s => s.GrupoMedio)
            .WithMany()
            .HasForeignKey(s => s.IdGrupoMedio)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
