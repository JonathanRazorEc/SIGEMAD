using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
public class ActuacionRelevanteDGPCEWithConvocatoriaCECOD : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public ActuacionRelevanteDGPCEWithConvocatoriaCECOD(ActuacionRelevanteDGPCESpecificationParams @params)
     : base(d =>
     (!@params.Id.HasValue || d.Id == @params.Id) &&
     (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
      d.Borrado == false)
    {
        AddInclude(d => d.ConvocatoriasCECOD);
    }
}
