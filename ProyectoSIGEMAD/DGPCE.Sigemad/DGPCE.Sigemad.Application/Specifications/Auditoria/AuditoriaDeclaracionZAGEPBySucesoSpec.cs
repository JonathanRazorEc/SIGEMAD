using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;

public class AuditoriaDeclaracionZAGEPBySucesoSpec : BaseSpecification<AuditoriaDeclaracionZAGEP>
{
    public AuditoriaDeclaracionZAGEPBySucesoSpec(int idSuceso)
        : base(c => c.ActuacionRelevanteDGPCE.IdSuceso == idSuceso && !c.Borrado)
    {
    }
}
