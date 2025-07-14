using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class TipoNotificacionConfiguration : IEntityTypeConfiguration<TipoNotificacion>
    {
        public void Configure(EntityTypeBuilder<TipoNotificacion> builder)
        {
            builder.ToTable(nameof(TipoNotificacion));

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Descripcion)
                .HasMaxLength(250);
              
        }
    }
}
