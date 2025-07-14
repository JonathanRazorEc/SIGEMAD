using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace DGPCE.Sigemad.Infrastructure.Configurations;
internal class DeclaracionZAGEPConfiguration : IEntityTypeConfiguration<DeclaracionZAGEP>
{
    public void Configure(EntityTypeBuilder<DeclaracionZAGEP> builder)
    {
        builder.ToTable(nameof(DeclaracionZAGEP));
        builder.HasKey(c => c.Id);
        builder.Property(e => e.IdActuacionRelevanteDGPCE)
             .IsRequired();
            
        builder.Property(e => e.Denominacion)
                .IsRequired()
               .HasMaxLength(510);

        // Configuración para `FechaSolicitud` con DateOnly
        builder.Property(d => d.FechaSolicitud)
            .HasConversion<DateOnlyConverter>()
            .IsRequired();

        builder.HasOne(d => d.ActuacionRelevanteDGPCE)
            .WithMany(dce => dce.DeclaracionesZAGEP)
            .HasForeignKey(d => d.IdActuacionRelevanteDGPCE)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
