using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
   public class FasesEmergenciaConfiguration : IEntityTypeConfiguration<FaseEmergencia>
    {
        public void Configure(EntityTypeBuilder<FaseEmergencia> builder)
        {
            builder.ToTable("FaseEmergencia");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Descripcion)
               .HasMaxLength(510)
               .IsUnicode(false);

            builder.HasOne(d => d.PlanEmergencia).WithMany()
                    .HasForeignKey(d => d.IdPlanEmergencia)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
