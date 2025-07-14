using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Vms;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Datos
{
    public class OpeDatoAsistenciaConfiguration : IEntityTypeConfiguration<OpeDatoAsistencia>
    {
        public void Configure(EntityTypeBuilder<OpeDatoAsistencia> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_DatoAsistencia");

            builder.HasOne(opeDatoAsistencia => opeDatoAsistencia.OpePuerto)
                   .WithMany()
                   .HasForeignKey(opeDatoAsistencia => opeDatoAsistencia.IdOpePuerto)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpeDatoAsistencia_OpePuerto");


            builder.HasMany(e => e.OpeDatosAsistenciasSanitarias)
                .WithOne(d => d.OpeDatoAsistencia)
                .HasForeignKey(d => d.IdOpeDatoAsistencia)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OpeDatosAsistenciasSanitarias_OpeDatoAsistencia");

            builder.HasMany(e => e.OpeDatosAsistenciasSociales)
               .WithOne(d => d.OpeDatoAsistencia)
               .HasForeignKey(d => d.IdOpeDatoAsistencia)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK_OpeDatosAsistenciasSociales_OpeDatoAsistencia");

            builder.HasMany(e => e.OpeDatosAsistenciasTraducciones)
                .WithOne(d => d.OpeDatoAsistencia)
                .HasForeignKey(d => d.IdOpeDatoAsistencia)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OpeDatosAsistenciasTraducciones_OpeDatoAsistencia");
        }
    }
}
