using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

public class OpePeriodo : BaseDomainModel<int>
{
    public string Nombre { get; set; } = null!;
    public int IdOpePeriodoTipo { get; set; }
    public DateTime FechaInicioFaseSalida { get; set; }
    public DateTime FechaFinFaseSalida { get; set; }
    public DateTime FechaInicioFaseRetorno { get; set; }
    public DateTime FechaFinFaseRetorno { get; set; }

    public virtual OpePeriodoTipo OpePeriodoTipo { get; set; } = null!;
}
