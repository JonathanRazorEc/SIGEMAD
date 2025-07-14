using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;

public class AuditoriaActivacionPlanEmergenciaBySucesoSpec : BaseSpecification<AuditoriaActivacionPlanEmergencia>
{
    public AuditoriaActivacionPlanEmergenciaBySucesoSpec(int idSuceso)
        : base(c => c.ActuacionRelevanteDGPCE.IdSuceso == idSuceso && !c.Borrado)
    {
        AddInclude(c => c.PlanEmergencia);
        AddInclude(c => c.TipoPlan);
    }
}
