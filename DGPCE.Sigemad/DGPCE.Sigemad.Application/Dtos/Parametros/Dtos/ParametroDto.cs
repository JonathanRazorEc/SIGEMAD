using DGPCE.Sigemad.Application.Dtos.SituacionesEquivalentes;
using DGPCE.Sigemad.Application.Features.Fases.Vms;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Vms;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.Parametros.Dtos;
public class ParametroDto
{
    public int Id { get; set; }

    public int idRegistro { get; set; }
    public DateTime? FechaFinal { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }

    public PlanEmergenciaVm? PlanEmergencia { get; set; }

    public EstadoIncendio? EstadoIncendio { get; set; }

    public FaseEmergenciaVm? FaseEmergencia { get; set; }
    public PlanSituacionVm? PlanSituacion { get; set; }
    public SituacionEquivalenteDto? SituacionEquivalente { get; set; }
    public DateTime? FechaHoraActualizacion { get; set; }

    public string? Observaciones { get; set; }

    public string? Prevision { get; set; }

    public bool EsEliminable { get; set; }

    // NUEVOS INDICADORES

    // AUDITORÍA REAL
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }

    public bool EsNuevo { get; set; }
    public bool EsModificado { get; set; }

}
