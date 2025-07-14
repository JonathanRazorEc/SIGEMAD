using NetTopologySuite.Geometries;


namespace DGPCE.Sigemad.Application.Features.Municipios.Vms;
public class MunicipioConIdProvincia
{
    public int Id { get; set; }

    public int IdProvincia { get; set; }

    public string Descripcion { get; set; } = null!;

    public int? UtmX { get; set; }

    public int? UtmY { get; set; }

    public string? Huso { get; set; }

    public Geometry? GeoPosicion { get; set; }
}
