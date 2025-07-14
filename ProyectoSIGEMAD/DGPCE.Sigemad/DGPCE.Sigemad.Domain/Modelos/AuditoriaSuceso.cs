using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;

public class AuditoriaSuceso: BaseDomainModel<int>
{
    public int IdTipo { get; set; }
    public virtual TipoSuceso TipoSuceso { get; set; } = null!;
    
}
