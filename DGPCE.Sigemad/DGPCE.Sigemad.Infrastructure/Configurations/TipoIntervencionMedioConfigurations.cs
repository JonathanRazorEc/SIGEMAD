
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class TipoIntervencionMedioConfigurations : IEntityTypeConfiguration<TipoIntervencionMedio>
    {
        public void Configure(EntityTypeBuilder<TipoIntervencionMedio> builder)
        {
            builder.ToTable("TipoIntervencionMedio");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(t => t.Descripcion)
                .IsUnique()
                .HasDatabaseName("IX_TipoMedioPresente");

            builder.HasOne(t => t.ClasificacionMedio)
                .WithMany()
                .HasForeignKey(t => t.IdClasificacion)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoMedioPresente_MediosClasificacion");

            builder.HasOne(t => t.TitularidadMedio)
                .WithMany()
                .HasForeignKey(t => t.IdTitularidad)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoMedioPresente_TitularidadMedio");

            builder.HasOne(t => t.TipoEntidadTitularidadMedio)
                .WithMany()
                .HasForeignKey(t => t.IdTitularidadEstatal)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoMedioPresente_TipoEntidadTitularidadMedio");

            builder.HasOne(t => t.TitularidadAutonomica)
                .WithMany()
                .HasForeignKey(t => t.IdTitularidadAutonomica)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoMedioPresente_CCAA");

            builder.HasOne(t => t.TitularidadAutonomicaMunicipal)
                .WithMany()
                .HasForeignKey(t => t.IdTitularidadAutonomicaMunicipal)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoMedioPresente_CCAA1");

            builder.HasOne(t => t.TitularidadProvinciaMunicipal)
                .WithMany()
                .HasForeignKey(t => t.IdTitularidadProvinciaMunicipal)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoMedioPresente_Provincia");

            builder.HasOne(t => t.TitularidadMunicipal)
                .WithMany()
                .HasForeignKey(t => t.IdTitularidadMunicipal)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoMedioPresente_Municipio");

            builder.HasOne(t => t.TitularidadPais)
                .WithMany()
                .HasForeignKey(t => t.IdTitularidadPais)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoMedioPresente_Pais");
        }
    }
  }
