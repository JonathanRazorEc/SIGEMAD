using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Administracion
{
    public class OpeFronteraConfiguration : IEntityTypeConfiguration<OpeFrontera>
    {
        public void Configure(EntityTypeBuilder<OpeFrontera> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_Frontera");

            // PK con 3 decimales
            builder.Property(e => e.PK)
           .HasColumnType("decimal(10,3)");
            //

            builder.HasOne(d => d.Ccaa).WithMany()
             .HasForeignKey(d => d.IdCcaa)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Provincia).WithMany()
           .HasForeignKey(d => d.IdProvincia)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Municipio).WithMany()
                .HasForeignKey(d => d.IdMunicipio)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
