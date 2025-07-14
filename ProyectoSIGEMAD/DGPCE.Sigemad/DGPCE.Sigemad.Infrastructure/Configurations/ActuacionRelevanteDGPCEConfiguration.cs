using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ActuacionRelevanteDGPCEConfiguration : IEntityTypeConfiguration<ActuacionRelevanteDGPCE>
{
    public void Configure(EntityTypeBuilder<ActuacionRelevanteDGPCE> builder)
    {
        builder.ToTable(nameof(ActuacionRelevanteDGPCE));
        builder.HasKey(c => c.Id);

        builder.HasOne(s => s.Suceso)
            .WithMany(a => a.ActuacionesRelevantes)
            .HasForeignKey(s => s.IdSuceso)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);


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

        // Configurar relación uno a uno con EmergenciaNacional
        builder.HasOne(a => a.EmergenciaNacional)
            .WithOne(e => e.ActuacionRelevanteDGPCE)
            .HasForeignKey<EmergenciaNacional>(r => r.Id) // El Id de EmergenciaNacional es también la clave foránea
            .OnDelete(DeleteBehavior.Cascade); // Configurar comportamiento de eliminación en cascada


        builder.HasMany(a => a.MovilizacionMedios)
            .WithOne(p => p.ActuacionRelevanteDGPCE)
            .HasForeignKey(p => p.IdActuacionRelevanteDGPCE)
            .OnDelete(DeleteBehavior.Restrict);

       // builder.HasOne(a => a.AuditoriaEmergenciaNacional)
       //.WithOne(e => e.ActuacionRelevanteDGPCE)
       //.HasForeignKey<AuditoriaEmergenciaNacional>(x => x.IdEmergencia);

        //builder.HasOne(a => a.AuditoriaActivacionSistemas)
        //    .WithMany(ar => ar.ActuacionRelevanteDGPCE).HasForeignKey(a => a.IdActuacionRelevanteDGPCE).HasConstraintName("FK_Auditoria_ActivacionSistema_ActuacionRelevanteDGPCE");


        //builder.HasMany(a => a.AuditoriaMovilizacionMedios)
        //    .WithOne(p => p.ActuacionRelevanteDGPCE)
        //    .HasForeignKey(p => p.IdActuacionRelevanteDGPCE)
        //    .OnDelete(DeleteBehavior.Restrict);
    }
}
