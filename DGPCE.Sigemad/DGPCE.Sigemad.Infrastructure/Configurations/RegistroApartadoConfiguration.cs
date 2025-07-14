using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class RegistroApartadoConfiguration : IEntityTypeConfiguration<RegistroApartado>
{
    public void Configure(EntityTypeBuilder<RegistroApartado> builder)
    {
        builder.ToTable("RegistroApartado");

        builder.HasKey(ra => ra.Id);

        builder.HasOne(ra => ra.RegistroActualizacion)
            .WithMany(r => r.RegistrosApartados)
            .HasForeignKey(ra => ra.IdRegistroActualizacion);

        builder.HasOne(ra => ra.ApartadoRegistro)
            .WithMany(a => a.RegistrosApartados)
            .HasForeignKey(ra => ra.IdApartadoRegistro);
    }
}

