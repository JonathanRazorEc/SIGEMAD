using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Datos
{
    public class OpeDatoAsistenciaSocialTareaConfiguration : IEntityTypeConfiguration<OpeDatoAsistenciaSocialTarea>
    {
        public void Configure(EntityTypeBuilder<OpeDatoAsistenciaSocialTarea> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_DatoAsistenciaSocialTarea");

            builder.Property(e => e.Observaciones)
                  .HasMaxLength(1000); // Por si quieres limitarlo


            // Relación con OpeDatoAsistenciaSocial
            builder.HasOne(d => d.OpeDatoAsistenciaSocial)
                .WithMany(p => p.OpeDatosAsistenciasSocialesTareas)
                .HasForeignKey(d => d.IdOpeDatoAsistenciaSocial)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesTareas_OpeDatoAsistenciaSocial");

            // Relación con OpeAsistenciaSocialTipo
            builder.HasOne(d => d.OpeAsistenciaSocialTareaTipo)
                .WithMany()
                .HasForeignKey(d => d.IdOpeAsistenciaSocialTareaTipo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesTareas_OpeAsistenciaSocialTareaTipo");  
        }
    }
}
