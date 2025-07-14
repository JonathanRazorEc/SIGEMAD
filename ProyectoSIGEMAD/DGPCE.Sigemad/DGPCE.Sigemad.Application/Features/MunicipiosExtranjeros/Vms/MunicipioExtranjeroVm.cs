using DGPCE.Sigemad.Application.Features.Distritos.Vms;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.MunicipiosExtranjeros.Vms;
public class MunicipioExtranjeroVm
{
    public int Id { get; set; }
    public string? CodigoOficial { get; set; }
    public string Descripcion { get; set; } = null!;
    public int? UtmX { get; set; }
    public int? UtmY { get; set; }
    public int? Huso { get; set; }
    public bool EsFronterizo { get; set; }
    public Geometry? GeoPosicion { get; set; }
    public virtual DistritoVm Distrito { get; set; } = null!;
}
