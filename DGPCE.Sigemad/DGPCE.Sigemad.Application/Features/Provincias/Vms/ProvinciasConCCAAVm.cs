using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Provincias.Vms;
public class ProvinciasConCCAAVm
{

    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;

    public int? UtmX { get; set; }

    public int? UtmY { get; set; }

    public string? Huso { get; set; }

    public Geometry? GeoPosicion { get; set; }
    public ComunidadesAutonomasConPaisVm ComunidadAutonoma { get; set; } = null!;
}
