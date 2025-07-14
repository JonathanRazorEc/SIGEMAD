
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergenciasByIdTipoPlan;
public class GetPlanesEmergenciasListByIdTipoPlanQuery : IRequest<IReadOnlyList<PlanEmergenciaVm>>
{
    public int IdTipoPlan { get; set; }

    public GetPlanesEmergenciasListByIdTipoPlanQuery(int idTipoPlan)
    {
        IdTipoPlan = idTipoPlan;
    }
}
