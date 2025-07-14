using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class SolicitudMedioConfiguration : IEntityTypeConfiguration<SolicitudMedio>
{
    public void Configure(EntityTypeBuilder<SolicitudMedio> builder)
    {
        builder.ToTable(nameof(SolicitudMedio));
        builder.HasKey(c => c.Id);

        builder.HasOne(builder => builder.ProcedenciaMedio)
            .WithMany()
            .HasForeignKey(builder => builder.IdProcedenciaMedio)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.Archivo)
            .WithMany()
            .HasForeignKey(builder => builder.IdArchivo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.EjecucionPaso)
            .WithOne(x => x.SolicitudMedio)
            .HasForeignKey<SolicitudMedio>(x => x.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
