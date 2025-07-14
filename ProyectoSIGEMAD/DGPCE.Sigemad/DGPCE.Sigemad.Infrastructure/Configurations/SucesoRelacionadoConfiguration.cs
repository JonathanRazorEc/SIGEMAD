using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class SucesoRelacionadoConfiguration : IEntityTypeConfiguration<SucesoRelacionado>
{
    public void Configure(EntityTypeBuilder<SucesoRelacionado> builder)
    {
        builder.ToTable("SucesoRelacionado");
        builder.HasKey(e => e.Id);

        builder.HasOne(s => s.SucesoPrincipal)
            .WithMany()
            .HasForeignKey(s => s.IdSucesoPrincipal)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(sr => sr.DetalleSucesoRelacionados)
            .WithOne(dsr => dsr.SucesoRelacionado)
            .HasForeignKey(dsr => dsr.IdCabeceraSuceso)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.SucesoPrincipal)
                .WithMany(i => i.SucesoRelacionados)
                .HasForeignKey(d => d.IdSucesoPrincipal)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
