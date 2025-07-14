using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Administracion
{
    public class OpePuertoConfiguration : IEntityTypeConfiguration<OpePuerto>
    {
        public void Configure(EntityTypeBuilder<OpePuerto> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_Puerto");

            builder.HasOne(opePuerto => opePuerto.OpeFase)
                   .WithMany()
                   .HasForeignKey(opePuerto => opePuerto.IdOpeFase)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpePuerto_OpeFase");
        }
    }
}
