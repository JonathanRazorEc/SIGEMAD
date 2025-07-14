using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DGPCE.Sigemad.Infrastructure.Configurations;
internal class RegistroConfiguration : IEntityTypeConfiguration<Registro>
{
    public void Configure(EntityTypeBuilder<Registro> builder)
    {

        builder.ToTable("Registro");

        builder.HasKey(e => e.Id);
        builder.HasQueryFilter(r => r.Borrado == false);

        builder.Property(e => e.FechaCreacion)
            .HasColumnType("datetime");

        builder.Property(e => e.FechaModificacion)
            .HasColumnType("datetime");

        builder.Property(e => e.FechaModificacion)
            .HasColumnType("datetime");

        builder.Property(e => e.CreadoPor)
          .HasMaxLength(500)
          .IsUnicode(false);

        builder.Property(e => e.ModificadoPor)
          .HasMaxLength(500)
          .IsUnicode(false);

        builder.Property(e => e.EliminadoPor)
        .HasMaxLength(500)
        .IsUnicode(false);

        builder.HasOne(d => d.Medio)
           .WithMany()
           .HasForeignKey(d => d.IdMedio)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.EntradaSalida)
            .WithMany()
            .HasForeignKey(d => d.IdEntradaSalida)
            .OnDelete(DeleteBehavior.Restrict);

        // Configurar relación uno a uno con Parametro
        builder.HasMany(e => e.Parametros)
            .WithOne(r => r.Registro)
            .HasForeignKey(r => r.IdRegistro) // El Id de Registro es también la clave foránea
            .OnDelete(DeleteBehavior.Cascade); // Configurar comportamiento de eliminación en cascada


        // Relación uno a uno con Suceso
        builder.HasOne(d => d.Suceso)
            .WithMany(s => s.Registros)
            .HasForeignKey(d => d.IdSuceso)
            .OnDelete(DeleteBehavior.Restrict); // Evita eliminación en cascada

        //builder.HasIndex(e => e.IdEvolucion)
        //    .IsUnique()
        //    .HasFilter("[Borrado] = 0");

        builder.HasMany(r => r.ProcedenciaDestinos)
            .WithOne(rpd => rpd.Registro)
            .HasForeignKey(rpd => rpd.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasMany(e => e.IntervencionMedios)
            .WithOne(r => r.Registro)
            .HasForeignKey(r => r.IdRegistro) // El Id de Registro es también la clave foránea
            .OnDelete(DeleteBehavior.Restrict); // Configurar comportamiento de eliminación en cascada



       builder.HasOne(d => d.CreadoPorNavigation)
       .WithMany()
       .HasForeignKey(d => d.CreadoPor)
       .OnDelete(DeleteBehavior.Restrict);

       builder.HasOne(d => d.EliminadoPorNavigation)
        .WithMany()
        .HasForeignKey(d => d.EliminadoPor)
        .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(d => d.ModificadoPorNavigation)
        .WithMany()
        .HasForeignKey(d => d.ModificadoPor)
        .OnDelete(DeleteBehavior.Restrict);

    }

}
