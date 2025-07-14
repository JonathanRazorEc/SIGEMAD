using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;

public class AuditoriaMovilizacionMedioBySucesoSpec : BaseSpecification<AuditoriaMovilizacionMedio>
{
    public AuditoriaMovilizacionMedioBySucesoSpec(int idSuceso)
        : base(c => c.ActuacionRelevanteDGPCE.IdSuceso == idSuceso && !c.Borrado)
    {
        AddInclude(c => c.Pasos);
        AddInclude("Pasos.AuditoriaSolicitudMedio");
    }
}
