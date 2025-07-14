
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.Provincias.Vms
{
    public class ProvinciaSinMunicipiosVm
    {

        public int Id { get; set; }

        public string Descripcion { get; set; } = null!;

        public int? UtmX { get; set; }

        public int? UtmY { get; set; }

        public string? Huso { get; set; }

        public Geometry? GeoPosicion { get; set; }

    }
}
