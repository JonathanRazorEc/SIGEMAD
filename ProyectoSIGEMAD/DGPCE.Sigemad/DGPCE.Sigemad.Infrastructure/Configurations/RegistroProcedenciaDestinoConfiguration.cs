

using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class RegistroProcedenciaDestinoConfiguration : IEntityTypeConfiguration<RegistroProcedenciaDestino>
{
    public void Configure(EntityTypeBuilder<RegistroProcedenciaDestino> builder)
    {

        builder.ToTable("Registro_ProcedenciaDestino");

        builder.HasKey(e => new {e.IdRegistro, e.IdProcedenciaDestino});

        builder.HasOne(rpd => rpd.Registro)
            .WithMany(r => r.ProcedenciaDestinos)
            .HasForeignKey(rpd => rpd.IdRegistro)
             .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(rpd => rpd.ProcedenciaDestino)
            .WithMany()
            .HasForeignKey(rpd => rpd.IdProcedenciaDestino);

    }
}
