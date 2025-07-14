
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.PlanesSituaciones.Queries.GetPlanesSituacionesListByIdPlanIdFase;
public class GetPlanesSituacionesByIdPlanIdFaseListQuery : IRequest<IReadOnlyList<PlanSituacionVm>>
{
    public int? IdPlanEmergencia { get; set; }
    public int? IdFaseEmergencia { get; set; }

    public GetPlanesSituacionesByIdPlanIdFaseListQuery(int? IdPlanEmergencia, int? IdFaseEmergencia)
    {
        this.IdPlanEmergencia = IdPlanEmergencia;
        this.IdFaseEmergencia = IdFaseEmergencia;
    }
}
