using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.PlanesSituaciones.Queries.GetPlanesByIdPlanAndSituacion;
public class GetPlanesByIdPlanAndSituacionQuery : IRequest<PlanSituacionConFaseVm>
{
    public int idPlan { get; set; }
    public char situacionEquivalente { get; set; }
    public GetPlanesByIdPlanAndSituacionQuery(int idPlan, char situacionEquivalente)
    {
        this.idPlan = idPlan;

        this.situacionEquivalente = situacionEquivalente;
    }
}
