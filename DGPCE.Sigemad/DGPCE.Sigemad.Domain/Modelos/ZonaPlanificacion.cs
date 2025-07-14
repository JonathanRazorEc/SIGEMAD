using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class ZonaPlanificacion : EditableCatalogModel
{
    public int Id { get; set; }
    public string Zona { get; set; }
    public int KmInicio { get; set; }
    public int KmFin { get; set; }
}
