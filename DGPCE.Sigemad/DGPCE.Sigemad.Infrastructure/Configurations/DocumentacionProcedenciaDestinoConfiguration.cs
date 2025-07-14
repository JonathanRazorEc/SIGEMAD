using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class DocumentacionProcedenciaDestinoConfiguration: IEntityTypeConfiguration<DocumentacionProcedenciaDestino>
{
    public void Configure(EntityTypeBuilder<DocumentacionProcedenciaDestino> builder)
    {
        builder.ToTable("Documentacion_ProcedenciaDestino");
        builder.HasKey(e => new {e.IdProcedenciaDestino, e.IdDetalleDocumentacion});

        builder.HasOne(e => e.DetalleDocumentacion)
                     .WithMany(e => e.DocumentacionProcedenciaDestinos)
                    .HasForeignKey(e => e.IdDetalleDocumentacion)
                    .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(d => d.ProcedenciaDestino)
               .WithMany()
               .HasForeignKey(d => d.IdProcedenciaDestino);
    }
}
