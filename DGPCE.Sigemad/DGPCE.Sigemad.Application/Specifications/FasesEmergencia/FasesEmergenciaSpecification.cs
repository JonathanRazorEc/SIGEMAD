using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.FasesEmergencia;
public class FasesEmergenciaSpecification : BaseSpecification<FaseEmergencia>
{
    public FasesEmergenciaSpecification(FasesEmergenciaParams request)
    : base(PlanEmergencia =>
         (string.IsNullOrEmpty(request.Descripcion) || PlanEmergencia.Descripcion.Contains(request.Descripcion)) &&
        (!request.Id.HasValue || PlanEmergencia.Id == request.Id) &&
        (!request.IdPlanEmergencia.HasValue || PlanEmergencia.IdPlanEmergencia == request.IdPlanEmergencia) 
        )
    {
        AddOrderBy(p => p.Orden);
    }
}
