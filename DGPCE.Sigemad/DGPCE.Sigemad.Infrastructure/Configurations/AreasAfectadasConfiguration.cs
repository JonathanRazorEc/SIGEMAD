using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class AreasAfectadasConfiguration : IEntityTypeConfiguration<AreaAfectada>
{
    public void Configure(EntityTypeBuilder<AreaAfectada> builder)
    {
        builder.ToTable("AreaAfectada");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.IdRegistro).IsRequired();
        builder.Property(a => a.FechaHora).IsRequired();
        builder.Property(a => a.IdProvincia).IsRequired();
        builder.Property(a => a.IdMunicipio).IsRequired();

        builder.Property(a => a.GeoPosicion)
            .HasColumnType("geometry");

        builder.HasOne(d => d.Registro)
            .WithMany(e => e.AreaAfectadas)
            .HasForeignKey(d => d.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Provincia).WithMany()
            .HasForeignKey(d => d.IdProvincia)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Municipio).WithMany()
        .HasForeignKey(d => d.IdMunicipio)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.EntidadMenor).WithMany()
            .HasForeignKey(d => d.IdEntidadMenor)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
