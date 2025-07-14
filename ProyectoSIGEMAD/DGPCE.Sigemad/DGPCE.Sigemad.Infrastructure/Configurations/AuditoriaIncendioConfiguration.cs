using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

internal class AuditoriaIncendioConfiguration : IEntityTypeConfiguration<Auditoria_Incendio>
{
    public void Configure(EntityTypeBuilder<Auditoria_Incendio> builder)
    {
        // Clave primaria
        builder.HasKey(e => e.Id);

        // Nombre de la tabla
        builder.ToTable("Auditoria_Incendio");

        // Ejemplo de configuración de columnas
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.Property(e => e.TipoMovimiento)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength(true);

        builder.Property(e => e.FechaRegistro)
            .HasColumnType("datetime2(7)");

        builder.Property(e => e.FechaInicio)
            .HasColumnType("datetime2(7)");

        builder.Property(e => e.Ubicacion)
            .HasMaxLength(510); // en la DB es nvarchar(510)

        builder.Property(e => e.Denominacion)
            .HasMaxLength(510)
            .IsRequired();      // NOT NULL en DB

        // etc. para todas las demás columnas si deseas afinar la configuración

        // ---------- Relaciones (Foreign Keys) ---------- //

        // IdIncendio -> Incendio(Id)
        builder.HasOne(d => d.Incendio)
            .WithMany()             // o .WithMany(x => x.AuditoriasIncendio) si la entidad Incendio tiene ICollection<Auditoria_Incendio>
            .HasForeignKey(d => d.IdIncendio)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Auditoria_Incendio_Incendio");

        // IdSuceso -> Suceso(Id)
        builder.HasOne(d => d.Suceso)
            .WithMany()
            .HasForeignKey(d => d.IdSuceso)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Auditoria_Incendio_Suceso");

        // IdTerritorio -> Territorio(Id)
        builder.HasOne(d => d.Territorio)
            .WithMany()
            .HasForeignKey(d => d.IdTerritorio)
            .OnDelete(DeleteBehavior.Restrict);

        // IdClaseSuceso -> ClaseSuceso(Id)
        builder.HasOne(d => d.ClaseSuceso)
            .WithMany()
            .HasForeignKey(d => d.IdClaseSuceso)
            .OnDelete(DeleteBehavior.Restrict);

        //IdEstadoSuceso->EstadoSuceso(Id)
        builder.HasOne(d => d.EstadoSuceso)
            .WithMany()
            .HasForeignKey(d => d.IdEstadoSuceso)
            .OnDelete(DeleteBehavior.Restrict);

        // IdPais -> Pais(Id)
        builder.HasOne(d => d.Pais)
            .WithMany()
            .HasForeignKey(d => d.IdPais)
            .OnDelete(DeleteBehavior.Restrict);

        // IdDistrito -> Distrito(Id)
        builder.HasOne(d => d.Distrito)
            .WithMany()
            .HasForeignKey(d => d.IdDistrito)
            .OnDelete(DeleteBehavior.Restrict);

        // IdMunicipioExtranjero -> MunicipioExtranjero(Id)
        builder.HasOne(d => d.MunicipioExtranjero)
            .WithMany()
            .HasForeignKey(d => d.IdMunicipioExtranjero)
            .OnDelete(DeleteBehavior.Restrict);

        // IdProvincia -> Provincia(Id)
        builder.HasOne(d => d.Provincia)
            .WithMany()
            .HasForeignKey(d => d.IdProvincia)
            .OnDelete(DeleteBehavior.Restrict);

        // IdMunicipio -> Municipio(Id)
        builder.HasOne(d => d.Municipio)
            .WithMany()
            .HasForeignKey(d => d.IdMunicipio)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Parametro)
            .WithMany()
            .HasForeignKey(d => d.IdEstadoSuceso)
            .OnDelete(DeleteBehavior.Restrict);

        // Opcionalmente, si quieres replicar el constraint TipoMovimiento IN('I','U','D'):
        // (Lo puedes manejar con un check constraint o en la lógica de negocio)
        // builder.HasCheckConstraint("CK_Auditoria_Incendio_TipoMov", "[TipoMovimiento] IN('I','U','D')");
    }
}
