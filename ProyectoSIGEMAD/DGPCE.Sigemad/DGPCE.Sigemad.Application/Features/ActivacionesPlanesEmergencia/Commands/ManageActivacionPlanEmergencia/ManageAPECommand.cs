using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
public class ManageActivacionPlanEmergenciaCommand : IRequest<ManageActivacionPlanEmergenciaResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<ManageActivacionPlanEmergenciaDto> ActivacionesPlanes { get; set; } = new();
}
