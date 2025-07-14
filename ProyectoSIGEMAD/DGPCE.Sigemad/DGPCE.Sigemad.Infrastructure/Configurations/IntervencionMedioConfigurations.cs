
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class IntervencionMedioConfigurations : IEntityTypeConfiguration<IntervencionMedio>
    {
        public void Configure(EntityTypeBuilder<IntervencionMedio> builder)
        {
            builder.ToTable("IntervencionMedio");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.GeoPosicion).HasColumnType("geometry");

            builder.HasOne(t => t.Registro)
                .WithMany(e => e.IntervencionMedios)
                .HasForeignKey(t => t.IdRegistro)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.CaracterMedio)
                .WithMany()
                .HasForeignKey(t => t.IdCaracterMedio)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.TitularidadMedio)
                .WithMany()
                .HasForeignKey(t => t.IdTitularidadMedio)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Municipio)
                .WithMany()
                .HasForeignKey(t => t.IdMunicipio)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Provincia)
                .WithMany()
                .HasForeignKey(t => t.IdProvincia)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Capacidad)
                 .WithMany()
                 .HasForeignKey(t => t.IdCapacidad)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
  }
