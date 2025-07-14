using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Datos
{
    public class OpeDatoAsistenciaSocialOrganismoConfiguration : IEntityTypeConfiguration<OpeDatoAsistenciaSocialOrganismo>
    {
        public void Configure(EntityTypeBuilder<OpeDatoAsistenciaSocialOrganismo> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_DatoAsistenciaSocialOrganismo");

            builder.Property(e => e.Observaciones)
                  .HasMaxLength(1000);


            // Relación con OpeDatoAsistenciaSocial
            builder.HasOne(d => d.OpeDatoAsistenciaSocial)
                .WithMany(p => p.OpeDatosAsistenciasSocialesOrganismos)
                .HasForeignKey(d => d.IdOpeDatoAsistenciaSocial)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesOrganismos_OpeDatoAsistenciaSocial");

            // Relación con OpeAsistenciaSocialTipo
            builder.HasOne(d => d.OpeAsistenciaSocialOrganismoTipo)
                .WithMany()
                .HasForeignKey(d => d.IdOpeAsistenciaSocialOrganismoTipo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OpeDatosAsistenciasSocialesOrganismos_OpeAsistenciaSocialOrganismoTipo");  
        }
    }
}
