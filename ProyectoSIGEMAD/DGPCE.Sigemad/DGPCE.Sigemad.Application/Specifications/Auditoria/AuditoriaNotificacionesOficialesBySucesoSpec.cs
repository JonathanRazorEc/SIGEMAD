using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;

public class AuditoriaNotificacionesOficialesBySucesoSpec : BaseSpecification<AuditoriaNotificacionEmergencia>
{
    public AuditoriaNotificacionesOficialesBySucesoSpec(int idSuceso)
        : base(c => c.ActuacionRelevanteDGPCE.IdSuceso == idSuceso && !c.Borrado)
    {
        AddInclude(c => c.TipoNotificacion);
    }
}
