using DGPCE.Sigemad.Application.Dtos.CaracterMedios;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Application.Dtos.Municipios;
using DGPCE.Sigemad.Application.Dtos.Provincias;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
public class IntervencionMedioDto
{
    public int Id { get; set; }

    public int idRegistro { get; set; }
    public CaracterMedioDto CaracterMedio { get; set; }
    public string? Descripcion { get; set; }
    public string? MedioNoCatalogado { get; set; }
    public int NumeroCapacidades { get; set; }
    public TitularidadMedioDto TitularidadMedio { get; set; }
    public string? Titular { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }
    public ProvinciaDto Provincia { get; set; }
    public MunicipioDto Municipio { get; set; }
    public Geometry? GeoPosicion { get; set; }
    public string? Observaciones { get; set; }
    public CapacidadDto Capacidad { get; set; }
    public List<DetalleIntervencionMedioDto> DetalleIntervencionMedios { get; set; } = new();
    public bool EsEliminable { get; set; }

    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    // NUEVOS INDICADORES
    public bool EsNuevo { get; set; }
    public bool EsModificado { get; set; }
}
