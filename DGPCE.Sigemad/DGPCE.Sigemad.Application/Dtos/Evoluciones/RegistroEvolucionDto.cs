using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.AspNetUsers;
using DGPCE.Sigemad.Application.Dtos.BaseEntity;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Dtos.Parametros.Dtos;
using DGPCE.Sigemad.Application.Dtos.ProcedenciasDestinos;
using DGPCE.Sigemad.Application.Dtos.TipoImpactoEvolucion;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.Evoluciones;
public class RegistroEvolucionDto:BaseEntityDto
{
    public DateTime FechaHoraEvolucion { get; set; }
    public Medio Medio { get; set; }
    public EntradaSalida EntradaSalida { get; set; }
    public List<ProcedenciaDto> ProcedenciaDestinos { get; set; }
    public List<AreaAfectadaDto> AreaAfectadas { get; set; } = new();
    public List<ParametroDto> Parametros { get; set; } = new();
    public List<TipoImpactoEvolucionDTO> TipoImpactosEvoluciones { get; set; } = new();

    public List<IntervencionMedioDto> IntervencionMedios { get; set; } = new();

    public List<DireccionDto> Direcciones { get; set; } = new();
    public List<CoordinacionCecopiDto> CoordinacionesCecopi { get; set; } = new();
    public List<CoordinacionPMADto> CoordinacionesPMA { get; set; } = new();

    public virtual List<ActivacionPlanEmergenciaDto> ActivacionPlanEmergencias { get; set; } = new();

    public virtual List<ActivacionSistemaDto> ActivacionSistemas { get; set; } = new();

    // public virtual List<ActivacionPlanEmergenciaDto> ActivacionPlanEmergenciasRegionales { get; set; } = new();

    //public virtual List<ActivacionPlanEmergenciaDto> ActivacionPlanEmergenciasEstatales { get; set; } = new();

    public bool RegistrosPosterioresConAreasAfectadas { get; set; }

    //nuevo modificado
    // AUDITORÍA REAL
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public bool EsNuevo { get; set; }
    public bool EsModificado { get; set; }

}
