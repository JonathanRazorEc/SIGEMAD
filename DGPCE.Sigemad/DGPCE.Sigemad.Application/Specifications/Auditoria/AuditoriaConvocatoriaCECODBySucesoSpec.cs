using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;

public class AuditoriaConvocatoriaCECODBySucesoSpec : BaseSpecification<AuditoriaConvocatoriaCECOD>
{
    public AuditoriaConvocatoriaCECODBySucesoSpec(int idSuceso)
        : base(c => c.ActuacionRelevanteDGPCE.IdSuceso == idSuceso && !c.Borrado)
    {
    }
}
