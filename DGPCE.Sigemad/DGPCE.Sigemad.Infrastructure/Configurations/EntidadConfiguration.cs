using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class EntidadConfiguration : IEntityTypeConfiguration<Entidad>
{
    public void Configure(EntityTypeBuilder<Entidad> builder)
    {
        builder.ToTable(nameof(Entidad));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.Organismo)
            .WithMany()
            .HasForeignKey(builder => builder.IdOrganismo)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
