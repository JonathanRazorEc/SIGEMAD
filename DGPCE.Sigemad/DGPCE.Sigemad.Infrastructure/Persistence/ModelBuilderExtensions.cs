using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Infrastructure.Persistence;
public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyHasTrigger(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName != null)
            {
                entityType.AddTrigger("trg_Auditoria_" + tableName);
            }
        }

        return modelBuilder;
    }
}
