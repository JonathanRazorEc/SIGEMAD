using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class TipoDireccionEmergenciaConfiguration : IEntityTypeConfiguration<TipoDireccionEmergencia>
    {
        public void Configure(EntityTypeBuilder<TipoDireccionEmergencia> builder)
        {
            builder.ToTable("TipoDireccionEmergencia");

            builder.HasKey(t => t.Id); ;

            builder.HasOne(t => t.TipoSuceso)
                .WithMany()
                .HasForeignKey(t => t.IdTipoSuceso)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
