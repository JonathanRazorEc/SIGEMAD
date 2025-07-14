using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Municipio : BaseDomainModel<int>
{
    public Municipio() { }

    public int IdProvincia { get; set; }

    public string Descripcion { get; set; } = null!;

    public int? UtmX { get; set; }

    public int? UtmY { get; set; }

    public string? Huso { get; set; }

    public Geometry? GeoPosicion { get; set; }

    public virtual Provincia Provincia { get; set; } = null!;
}
