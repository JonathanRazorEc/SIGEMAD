using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Vms;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Datos
{
    public class OpeDatoAsistenciaSanitariaConfiguration : IEntityTypeConfiguration<OpeDatoAsistenciaSanitaria>
    {
        public void Configure(EntityTypeBuilder<OpeDatoAsistenciaSanitaria> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_DatoAsistenciaSanitaria");

            builder.Property(e => e.Observaciones)
                  .HasMaxLength(1000);


            // Relación con OpeDatoAsistencia
            builder.HasOne(d => d.OpeDatoAsistencia)
                .WithMany(p => p.OpeDatosAsistenciasSanitarias)
                .HasForeignKey(d => d.IdOpeDatoAsistencia)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OpeDatosAsistenciasSanitarias_OpeDatoAsistencia");

            // Relación con OpeAsistenciaSanitariaTipo
            builder.HasOne(d => d.OpeAsistenciaSanitariaTipo)
                .WithMany()
                .HasForeignKey(d => d.IdOpeAsistenciaSanitariaTipo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OpeDatosAsistenciasSanitarias_OpeAsistenciaSanitariaTipo");
        }
    }
}
