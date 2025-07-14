using System.Text.Json;
using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

public class OpePorcentajeOcupacionAreaEstacionamiento : BaseDomainModel<int>
{
    public int IdOpeOcupacion { get; set; }
    public int PorcentajeInferior { get; set; }
    public int PorcentajeSuperior { get; set; }

    public virtual OpeOcupacion OpeOcupacion { get; set; } = null!;
}
