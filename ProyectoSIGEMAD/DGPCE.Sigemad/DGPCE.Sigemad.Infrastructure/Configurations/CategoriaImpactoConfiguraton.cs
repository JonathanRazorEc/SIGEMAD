using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class CategoriaImpactoConfiguraton : IEntityTypeConfiguration<CategoriaImpacto>
{
    public void Configure(EntityTypeBuilder<CategoriaImpacto> builder)
    {
        builder.ToTable(nameof(CategoriaImpacto));

        builder.Property(e => e.Id)
        .UseIdentityColumn();

        builder.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(100);

        builder.HasOne(d => d.SubgrupoImpacto).WithMany()
            .HasForeignKey(d => d.IdSubgrupoImpacto)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
