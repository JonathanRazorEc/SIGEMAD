using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;
public class MunicipioExtranjero : BaseDomainModel<int>
{
    public int IdDistrito { get; set; }
    public string? CodigoOficial { get; set; }
    public string Descripcion { get; set; } = null!;
    public int? UtmX { get; set; }
    public int? UtmY { get; set; }
    public int? Huso { get; set; }
    public bool EsFronterizo { get; set; }
    public Geometry? GeoPosicion { get; set; }


    public virtual Distrito Distrito { get; set; } = null!;
}
