using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;
public class IntervencionMedio : BaseDomainModel<int>
{
    public int IdRegistro { get; set; }
    public Registro Registro { get; set; }

    
    public int IdCaracterMedio { get; set; }
    public CaracterMedio CaracterMedio { get; set; }


    public string? Descripcion { get; set; }
    public string? MedioNoCatalogado { get; set; }
    public int NumeroCapacidades { get; set; }


    public int IdTitularidadMedio { get; set; }
    public TitularidadMedio TitularidadMedio { get; set; }


    public string? Titular { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }


    public int? IdProvincia { get; set; }
    public Provincia Provincia { get; set; }

    public int? IdMunicipio { get; set; }
    public Municipio Municipio { get; set; }
    
    public Geometry? GeoPosicion { get; set; }
    public string? Observaciones { get; set; }


    public int IdCapacidad { get; set; }
    public Capacidad Capacidad { get; set; }

    public List<DetalleIntervencionMedio> DetalleIntervencionMedios { get; set; } = new();
}
