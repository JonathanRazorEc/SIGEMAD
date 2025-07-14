using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.PlanesSituaciones;
public class PlanesSituacionesParams
{
    public int? Id { get; set; }

    public int? IdPlanEmergencia { get; set; }
    public int? IdFaseEmergencia { get; set; }
    public string? Descripcion { get; set; }

    public string? Nivel { get; set; }

    public string? Situacion { get; set; }
    public string? SituacionEquivalente { get; set; }
}
