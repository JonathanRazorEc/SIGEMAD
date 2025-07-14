using DGPCE.Sigemad.Application.Features.Fases.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Fases.Queries.GetFasesEmergenciaListByIdPlanEmergencia;
public class GetFasesEmergenciaByIdPlanEmergenciaListQuery : IRequest<IReadOnlyList<FaseEmergenciaVm>>
{
    public int? IdPlanEmergencia { get; set; }

    public GetFasesEmergenciaByIdPlanEmergenciaListQuery(int? IdPlanEmergencia)
    {
        this.IdPlanEmergencia = IdPlanEmergencia;
    }
}
