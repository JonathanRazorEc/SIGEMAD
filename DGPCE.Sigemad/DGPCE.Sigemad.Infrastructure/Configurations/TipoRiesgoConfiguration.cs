using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class TipoRiesgoConfiguration : IEntityTypeConfiguration<TipoRiesgo>
    {
        public void Configure(EntityTypeBuilder<TipoRiesgo> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("TipoRiesgo");

            builder.HasOne(d => d.TipoSuceso).WithMany()
                    .HasForeignKey(d => d.IdTipoSuceso)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
