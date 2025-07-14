using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AuditoriaSolicitudMedioConfiguration : IEntityTypeConfiguration<AuditoriaSolicitudMedio>
{
    public void Configure(EntityTypeBuilder<AuditoriaSolicitudMedio> builder)
    {
        builder.ToTable(nameof(AuditoriaSolicitudMedio));
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

        builder.HasOne(x => x.AuditoriaEjecucionPaso)
            .WithOne(x => x.AuditoriaSolicitudMedio)
            .HasForeignKey<AuditoriaSolicitudMedio>(x => x.IdEjecucionPaso)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
