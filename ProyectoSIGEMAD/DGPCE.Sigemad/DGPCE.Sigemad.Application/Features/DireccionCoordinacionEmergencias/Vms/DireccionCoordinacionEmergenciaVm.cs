using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;


namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
public class DireccionCoordinacionEmergenciaVm : BaseDomainModel<int>
{
    public int IdIncendio { get; set; }
    public int IdTipoDireccionEmergencia { get; set; }
    public string AutoridadQueDirige { get; set; } = null!;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public DateTime? FechaInicioCECOPI { get; set; }
    public DateTime? FechaFinCECOPI { get; set; }
    public int IdProvinciaCECOPI { get; set; }
    public int IdMunicipioCECOPI { get; set; }
    public string? LugarCECOPI { get; set; }
    public Geometry? GeoPosicionCECOPI { get; set; }
    public string? ObservacionesCECOPI { get; set; }

    public DateTime FechaInicioPMA { get; set; }
    public DateTime? FechaFinPMA { get; set; }
    public int IdProvinciaPMA { get; set; }
    public int IdMunicipioPMA { get; set; }
    public string? LugarPMA { get; set; }
    public Geometry? GeoPosicionPMA { get; set; }
    public string? ObservacionesPMA { get; set; }
}
