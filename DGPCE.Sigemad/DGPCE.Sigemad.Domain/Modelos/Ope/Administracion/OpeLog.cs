using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

public class OpeLog : BaseDomainModel<int>
{
    public DateTime FechaRegistro { get; set; }
    public string TipoMovimiento { get; set; } = null!;

}
