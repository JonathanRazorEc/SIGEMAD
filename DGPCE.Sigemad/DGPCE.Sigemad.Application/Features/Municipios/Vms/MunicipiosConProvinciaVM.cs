using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.Municipios.Vms;
public class MunicipiosConProvinciaVM
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;

    public int? UtmX { get; set; }

    public int? UtmY { get; set; }

    public string? Huso { get; set; }

    public Geometry? GeoPosicion { get; set; }

    public virtual ProvinciasConCCAAVm Provincia { get; set; } = null!;
}
