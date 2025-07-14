using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Specifications.PlanesEmergencias;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergencias;
public class GetPlanesEmergenciasQuery : PlanesEmegenciasParams, IRequest<IReadOnlyList<PlanEmergenciaVm>>
{
    public bool IsFullDescription { get; set; } = false;
}
