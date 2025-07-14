using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class TipoMedioConfiguration : IEntityTypeConfiguration<TipoMedio>
{
    public void Configure(EntityTypeBuilder<TipoMedio> builder)
    {
        builder.ToTable("TipoMedio");

        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.SubgrupoMedio)
            .WithMany()
            .HasForeignKey(d => d.IdSubgrupoMedio)
            .OnDelete(DeleteBehavior.Restrict);

    }

}