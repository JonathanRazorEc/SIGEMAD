using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Datos
{
    public class OpeDatoAsistenciaSocialConfiguration : IEntityTypeConfiguration<OpeDatoAsistenciaSocial>
    {
        public void Configure(EntityTypeBuilder<OpeDatoAsistenciaSocial> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_DatoAsistenciaSocial");

            builder.Property(e => e.Observaciones)
                  .HasMaxLength(1000);


            // Relación con OpeDatoAsistencia
            builder.HasOne(d => d.OpeDatoAsistencia)
                .WithMany(p => p.OpeDatosAsistenciasSociales)
                .HasForeignKey(d => d.IdOpeDatoAsistencia)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OpeDatosAsistenciasSociales_OpeDatoAsistencia");

            // Relación con OpeAsistenciaSocialTipo
            builder.HasOne(d => d.OpeAsistenciaSocialTipo)
                .WithMany()
                .HasForeignKey(d => d.IdOpeAsistenciaSocialTipo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OpeDatosAsistenciasSociales_OpeAsistenciaSocialTipo");

            //
            builder.HasMany(e => e.OpeDatosAsistenciasSocialesTareas)
                .WithOne(d => d.OpeDatoAsistenciaSocial)
                .HasForeignKey(d => d.IdOpeDatoAsistenciaSocial)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesTareas_OpeDatoAsistenciaSocial");

            builder.HasMany(e => e.OpeDatosAsistenciasSocialesOrganismos)
                .WithOne(d => d.OpeDatoAsistenciaSocial)
                .HasForeignKey(d => d.IdOpeDatoAsistenciaSocial)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesOrganismos_OpeDatoAsistenciaSocial");

            /*
            builder.HasMany(e => e.OpeDatosAsistenciasSocialesUsuarios)
               .WithOne(d => d.OpeDatoAsistenciaSocial)
               .HasForeignKey(d => d.IdOpeDatoAsistenciaSocial)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK_OpeDatosAsistenciasSocialesUsuarios_OpeDatoAsistenciaSocial");
            */
            //
        }
    }
}
