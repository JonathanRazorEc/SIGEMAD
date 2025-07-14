using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;

public class AuditoriaActivacionSistemaBySucesoSpec : BaseSpecification<AuditoriaActivacionSistema>
{
    public AuditoriaActivacionSistemaBySucesoSpec(int idSuceso)
        : base(c => c.ActuacionRelevanteDGPCE.IdSuceso == idSuceso && !c.Borrado)
    {
        AddInclude(c => c.TipoSistemaEmergencia);
    }
}
