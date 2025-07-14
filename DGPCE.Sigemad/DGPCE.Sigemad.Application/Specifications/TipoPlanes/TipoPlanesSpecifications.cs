using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.TipoPlanes;

public class TipoPlanesSpecifications : BaseSpecification<PlanEmergencia>
{
    public TipoPlanesSpecifications(int idAmbito, int idTipoRiesgo)
    : base(r => r.IdAmbitoPlan == idAmbito && (r.IdTipoRiesgo == idTipoRiesgo || r.IdTipoRiesgo == 1))
    {
        AddInclude(r => r.TipoPlan);
    }
}
