using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Administracion
{
    public class OpePaisConfiguration : IEntityTypeConfiguration<OpePais>
    {
        public void Configure(EntityTypeBuilder<OpePais> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_Pais");

            builder.HasOne(opePais => opePais.Pais)
                   .WithMany()
                   .HasForeignKey(opePais => opePais.IdPais)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpePais_Pais");
        }
    }
}
