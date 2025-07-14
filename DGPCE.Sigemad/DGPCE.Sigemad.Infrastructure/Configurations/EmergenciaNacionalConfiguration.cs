using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class EmergenciaNacionalConfiguration : IEntityTypeConfiguration<EmergenciaNacional>
{
    public void Configure(EntityTypeBuilder<EmergenciaNacional> builder)
    {
        builder.ToTable(nameof(EmergenciaNacional));
        builder.HasKey(c => c.Id);

        builder.Property(e => e.Autoridad)
                .IsRequired()
               .HasMaxLength(510);

        builder.Property(e => e.DescripcionSolicitud)
                .IsRequired()
               .HasMaxLength(510);

        builder.Property(e => e.FechaHoraSolicitud).IsRequired().HasColumnType("datetime");

        builder.Property(e => e.DescripcionDeclaracion)
               .HasMaxLength(510);

        builder.Property(e => e.FechaHoraDeclaracion).HasColumnType("datetime");
        builder.Property(e => e.FechaHoraDireccion).HasColumnType("datetime");

        builder.Property(e => e.CreadoPor)
                 .HasMaxLength(500)
                 .IsUnicode(false);

        builder.Property(e => e.FechaCreacion).HasColumnType("datetime");
        builder.Property(e => e.FechaModificacion).HasColumnType("datetime");
        builder.Property(e => e.FechaEliminacion).HasColumnType("datetime");
        builder.Property(e => e.ModificadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(e => e.EliminadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);

        // Relación uno a uno con ActuacionRelevanteDGPCEConfiguration        
        builder.HasOne(r => r.ActuacionRelevanteDGPCE)
            .WithOne(e => e.EmergenciaNacional)
            .HasForeignKey<EmergenciaNacional>(r => r.Id); // El Id de EmergenciaNacional es también la clave foránea

    }
}
