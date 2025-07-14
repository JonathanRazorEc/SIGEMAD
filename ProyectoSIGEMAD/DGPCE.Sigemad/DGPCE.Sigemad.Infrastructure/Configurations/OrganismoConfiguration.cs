using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class OrganismoConfiguration : IEntityTypeConfiguration<Organismo>
{
    public void Configure(EntityTypeBuilder<Organismo> builder)
    {
        builder.ToTable(nameof(Organismo));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.Administracion)
            .WithMany()
            .HasForeignKey(builder => builder.IdAdministracion)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
