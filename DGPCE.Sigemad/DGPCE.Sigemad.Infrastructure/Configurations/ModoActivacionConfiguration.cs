using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class ModoActivacionConfiguration : IEntityTypeConfiguration<ModoActivacion>
    {
        public void Configure(EntityTypeBuilder<ModoActivacion> builder)
        {
            builder.ToTable("ModoActivacion");
            builder.HasKey(e => e.Id);

          builder.Property(e => e.Descripcion)
             .HasMaxLength(510)
             .IsUnicode(false);
        }
    }
}