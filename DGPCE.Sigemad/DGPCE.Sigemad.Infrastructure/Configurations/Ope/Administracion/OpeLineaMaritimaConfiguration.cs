using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Administracion
{
    public class OpeLineaMaritimaConfiguration : IEntityTypeConfiguration<OpeLineaMaritima>
    {
        public void Configure(EntityTypeBuilder<OpeLineaMaritima> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_LineaMaritima");

            builder.HasOne(opeLineaMaritima => opeLineaMaritima.OpePuertoOrigen)
                  .WithMany()
                  .HasForeignKey(opeLineaMaritima => opeLineaMaritima.IdOpePuertoOrigen)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_OpeLineaMaritima_OpePuertoOrigen");

            builder.HasOne(opeLineaMaritima => opeLineaMaritima.OpePuertoDestino)
                 .WithMany()
                 .HasForeignKey(opeLineaMaritima => opeLineaMaritima.IdOpePuertoDestino)
                 .OnDelete(DeleteBehavior.Restrict)
                 .HasConstraintName("FK_OpeLineaMaritima_OpePuertoDestino");

            builder.HasOne(opeLineaMaritima => opeLineaMaritima.OpeFase)
                   .WithMany()
                   .HasForeignKey(opeLineaMaritima => opeLineaMaritima.IdOpeFase)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpeLineaMaritima_OpeFase");
        }
    }
}
