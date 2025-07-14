using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
   public class TiposSistemasEmergenciasConfiguration : IEntityTypeConfiguration<TipoSistemaEmergencia>
    {
        public void Configure(EntityTypeBuilder<TipoSistemaEmergencia> builder)
        {
            builder.ToTable(nameof(TipoSistemaEmergencia));
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Descripcion)
               .HasMaxLength(510)
               .IsUnicode(false);
        }
    }

