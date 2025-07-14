using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Datos
{
    public class OpeDatoFronteraConfiguration : IEntityTypeConfiguration<OpeDatoFrontera>
    {
        public void Configure(EntityTypeBuilder<OpeDatoFrontera> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_DatoFrontera");

            builder.HasOne(opeDatoFrontera => opeDatoFrontera.OpeFrontera)
                   .WithMany()
                   .HasForeignKey(opeDatoFrontera => opeDatoFrontera.IdOpeFrontera)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpeDatoFrontera_OpeFrontera");

            builder.HasOne(opeDatoFrontera => opeDatoFrontera.OpeDatoFronteraIntervaloHorario)
                   .WithMany()
                   .HasForeignKey(opeDatoFrontera => opeDatoFrontera.IdOpeDatoFronteraIntervaloHorario)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpeDatoFrontera_OpeDatoFronteraIntervaloHorario");
        }
    }
}
