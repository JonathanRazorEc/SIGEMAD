using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;

public class AuditoriaEmergenciaNacionalBySucesoSpec : BaseSpecification<AuditoriaEmergenciaNacional>
{
    public AuditoriaEmergenciaNacionalBySucesoSpec(int idSuceso)
        : base(c => c.ActuacionRelevanteDGPCE.IdSuceso == idSuceso && !c.Borrado)
    {
    }
}
