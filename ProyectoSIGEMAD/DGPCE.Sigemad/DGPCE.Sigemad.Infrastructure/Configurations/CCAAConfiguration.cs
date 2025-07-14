using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class CCAAConfiguration : IEntityTypeConfiguration<Ccaa>
    {
        public void Configure(EntityTypeBuilder<Ccaa> builder)
        {

            builder.HasKey(e => e.Id).HasName("CCAA_PK");

            builder.ToTable("CCAA");

            builder.HasMany(c => c.Provincia).WithOne(p => p.IdCcaaNavigation)
             .HasForeignKey(p => p.IdCcaa);

            builder.HasOne(c => c.Pais)
                .WithMany()
                .HasForeignKey(c => c.IdPais)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
