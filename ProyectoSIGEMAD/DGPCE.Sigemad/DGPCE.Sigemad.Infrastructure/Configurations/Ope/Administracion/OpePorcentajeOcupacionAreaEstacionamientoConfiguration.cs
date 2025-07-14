using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Administracion
{
    public class OpePorcentajeOcupacionAreaEstacionamientoConfiguration : IEntityTypeConfiguration<OpePorcentajeOcupacionAreaEstacionamiento>
    {
        public void Configure(EntityTypeBuilder<OpePorcentajeOcupacionAreaEstacionamiento> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_PorcentajeOcupacionAreaEstacionamiento");

            builder.HasOne(opePorcentajeOcupacionAreaEstacionamiento => opePorcentajeOcupacionAreaEstacionamiento.OpeOcupacion)
                   .WithMany()
                   .HasForeignKey(opePorcentajeOcupacionAreaEstacionamiento => opePorcentajeOcupacionAreaEstacionamiento.IdOpeOcupacion)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpePorcentajeOcupacionAreaEstacionamiento_OpeOcupacion");
        }
    }
}
