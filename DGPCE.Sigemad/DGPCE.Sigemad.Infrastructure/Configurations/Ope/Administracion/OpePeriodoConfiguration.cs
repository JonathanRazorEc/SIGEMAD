using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Administracion
{
    public class OpePeriodoConfiguration : IEntityTypeConfiguration<OpePeriodo>
    {
        public void Configure(EntityTypeBuilder<OpePeriodo> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_Periodo");

            builder.HasOne(opePeriodo => opePeriodo.OpePeriodoTipo)
                   .WithMany()
                   .HasForeignKey(opePeriodo => opePeriodo.IdOpePeriodoTipo)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpePeriodo_OpePeriodoTipo");
        }
    }
}
