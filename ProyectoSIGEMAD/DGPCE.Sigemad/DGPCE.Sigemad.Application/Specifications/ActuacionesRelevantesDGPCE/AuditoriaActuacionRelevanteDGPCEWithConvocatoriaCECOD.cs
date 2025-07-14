using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
public class AuditoriaActuacionRelevanteDGPCEWithConvocatoriaCECOD : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public AuditoriaActuacionRelevanteDGPCEWithConvocatoriaCECOD(ActuacionRelevanteDGPCESpecificationParams @params)
     : base(d =>
     (!@params.Id.HasValue || d.Id == @params.Id) &&
     (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
      d.Borrado == false)
    {
        AddInclude(d => d.AuditoriaConvocatoriasCECOD);
    }
}
