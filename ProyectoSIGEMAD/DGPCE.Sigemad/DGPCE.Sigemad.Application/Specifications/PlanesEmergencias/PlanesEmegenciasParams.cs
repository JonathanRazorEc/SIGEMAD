using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.PlanesEmergencias;
public class PlanesEmegenciasParams
{

    public int? Id { get; set; }
    public string? Codigo { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int? IdCcaa { get; set; }

    public int? IdProvincia { get; set; }

    public int? IdMunicipio { get; set; }

    public int? IdTipoPlan { get; set; }
    public int? IdTipoRiesgo { get; set; }

    public int? IdTipoSuceso { get; set; }

    public int? IdAmbitoPlan { get; set; }
}
