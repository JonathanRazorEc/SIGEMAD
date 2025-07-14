using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Datos
{
    public class OpeDatoAsistenciaSocialUsuarioConfiguration : IEntityTypeConfiguration<OpeDatoAsistenciaSocialUsuario>
    {
        public void Configure(EntityTypeBuilder<OpeDatoAsistenciaSocialUsuario> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_DatoAsistenciaSocialUsuario");

            builder.Property(e => e.Observaciones)
                  .HasMaxLength(1000); // Por si quieres limitarlo


            // Relación con OpeDatoAsistenciaSocial
            builder.HasOne(d => d.OpeDatoAsistenciaSocial)
                .WithMany(p => p.OpeDatosAsistenciasSocialesUsuarios)
                .HasForeignKey(d => d.IdOpeDatoAsistenciaSocial)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesUsuarios_OpeDatoAsistenciaSocial");

            // Relación con Edad
            builder.HasOne(d => d.OpeAsistenciaSocialEdad)
                .WithMany()
                .HasForeignKey(d => d.IdOpeAsistenciaSocialEdad)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesUsuarios_OpeAsistenciaSocialEdad");

            // Relación con Sexo
            builder.HasOne(d => d.OpeAsistenciaSocialSexo)
                .WithMany()
                .HasForeignKey(d => d.IdOpeAsistenciaSocialSexo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesUsuarios_OpeAsistenciaSocialSexo");

            // Relación con nacionalidad
            builder.HasOne(d => d.OpeAsistenciaSocialNacionalidad)
                .WithMany()
                .HasForeignKey(d => d.IdOpeAsistenciaSocialNacionalidad)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesUsuarios_OpeAsistenciaSocialNacionalidad");

            // Relación con nacionalidad
            builder.HasOne(d => d.PaisResidencia)
                .WithMany()
                .HasForeignKey(d => d.IdPaisResidencia)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesUsuarios_Pais");
        }
    }
}
