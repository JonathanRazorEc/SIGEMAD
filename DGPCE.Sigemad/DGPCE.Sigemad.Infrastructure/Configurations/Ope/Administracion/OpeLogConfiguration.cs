using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Administracion
{
    public class OpeLogConfiguration : IEntityTypeConfiguration<OpeLog>
    {
        public void Configure(EntityTypeBuilder<OpeLog> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("Auditoria_OPE_Frontera");

            /*
            builder.HasOne(opeLog => opeLog.OpeLogTipo)
                   .WithMany()
                   .HasForeignKey(opeLog => opeLog.IdOpeLogTipo)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpeLog_OpeLogTipo");
            */
        }
    }
}
