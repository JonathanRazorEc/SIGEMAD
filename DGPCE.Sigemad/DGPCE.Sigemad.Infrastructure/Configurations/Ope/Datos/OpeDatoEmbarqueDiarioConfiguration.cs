using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Datos
{
    public class OpeDatoEmbarqueDiarioConfiguration : IEntityTypeConfiguration<OpeDatoEmbarqueDiario>
    {
        public void Configure(EntityTypeBuilder<OpeDatoEmbarqueDiario> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_DatoEmbarqueDiario");

            builder.HasOne(opeDatoEmbarqueDiario => opeDatoEmbarqueDiario.OpeLineaMaritima)
                   .WithMany()
                   .HasForeignKey(opeDatoEmbarqueDiario => opeDatoEmbarqueDiario.IdOpeLineaMaritima)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpeDatoEmbarqueDiario_OpeLineaMaritima");
        }
    }
}
