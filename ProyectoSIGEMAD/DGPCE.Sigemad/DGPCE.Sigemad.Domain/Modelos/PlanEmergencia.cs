using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos;
public class PlanEmergencia
{

    public int Id { get; set; }
    public string Codigo { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int? IdCcaa { get; set; }

    public int? IdProvincia { get; set; }

    public int? IdMunicipio { get; set; }

    public int IdTipoPlan { get; set; }
    public int IdTipoRiesgo { get; set; }

    public int IdAmbitoPlan { get; set; }

    public virtual Ccaa Ccaa { get; set; } = null!;
    public virtual Provincia Provincia { get; set; } = null!;
    public virtual Municipio Municipio { get; set; } = null!;

    public virtual TipoPlan TipoPlan { get; set; } = null!;

    public virtual TipoRiesgo TipoRiesgo { get; set; } = null!;

    public virtual AmbitoPlan AmbitoPlan { get; set; } = null!;
}
