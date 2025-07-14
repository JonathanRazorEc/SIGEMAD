using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Dtos.Parametros.Dtos;
using DGPCE.Sigemad.Application.Dtos.TipoImpactoEvolucion;


namespace DGPCE.Sigemad.Application.Dtos.RegistrosAnteriores;
public class RegistrosAnterioresDto
{
    public List<AreaAfectadaDto> AreaAfectadas { get; set; } = new();

    public List<ParametroDto> Parametros { get; set; } = new();

    public List<TipoImpactoEvolucionDTO> TipoImpactosEvoluciones { get; set; } = new();

    public List<IntervencionMedioDto> IntervencionMedios { get; set; } = new();

    public List<DireccionDto> Direcciones { get; set; } = new();
    public List<CoordinacionCecopiDto> CoordinacionesCecopi { get; set; } = new();
    public List<CoordinacionPMADto> CoordinacionesPMA { get; set; } = new();

    public virtual List<ActivacionPlanEmergenciaDto> ActivacionPlanEmergencias { get; set; } = new();
    public virtual List<ActivacionSistemaDto> ActivacionSistemas { get; set; } = new();

    //public virtual List<ActivacionPlanEmergenciaDto> ActivacionPlanEmergenciasRegionales { get; set; } = new();

    //public virtual List<ActivacionPlanEmergenciaDto> ActivacionPlanEmergenciasEstatales { get; set; } = new();
}
