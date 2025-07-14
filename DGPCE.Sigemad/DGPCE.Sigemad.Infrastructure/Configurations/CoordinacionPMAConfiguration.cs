using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class CoordinacionPMAConfiguration : IEntityTypeConfiguration<CoordinacionPMA>
{
    public void Configure(EntityTypeBuilder<CoordinacionPMA> builder)
    {
        builder.ToTable("CoordinacionPMA");

        builder.HasKey(e => e.Id);

        // Propiedades existentes
        builder.Property(e => e.FechaInicio)
            .IsRequired();

        builder.Property(e => e.FechaFin)
            .IsRequired(false);

        builder.Property(e => e.Lugar)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Observaciones)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);

        // AGREGAR: Configuración de campos UTM
        builder.Property(e => e.UTM_X)
            .HasColumnType("decimal(18,9)")
            .IsRequired(false);

        builder.Property(e => e.UTM_Y)
            .HasColumnType("decimal(18,9)")
            .IsRequired(false);

        builder.Property(e => e.Huso)
            .IsRequired(false);

        builder.Property(e => e.GeoPosicion)
            .HasColumnType("geometry")
            .IsRequired(false);

        // Relaciones existentes
        builder.HasOne(e => e.Registro)
            .WithMany(r => r.CoordinacionesPMA)
            .HasForeignKey(e => e.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Provincia)
            .WithMany()
            .HasForeignKey(e => e.IdProvincia)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Municipio)
            .WithMany()
            .HasForeignKey(e => e.IdMunicipio)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Archivo)
            .WithMany()
            .HasForeignKey(e => e.IdArchivo)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // AGREGAR: Constraint para validar Huso UTM (zonas típicas de España)
        builder.HasCheckConstraint("CK_CoordinacionPMA_Huso",
            "[Huso] IS NULL OR [Huso] BETWEEN 28 AND 31");
    }



}
