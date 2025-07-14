using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
internal class ActuacionRelevanteDGPCEWithDeclaracionesZAGEP : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public ActuacionRelevanteDGPCEWithDeclaracionesZAGEP(ActuacionRelevanteDGPCESpecificationParams @params)
   : base(d =>
   (!@params.Id.HasValue || d.Id == @params.Id) &&
   (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
    d.Borrado == false)
    {
        AddInclude(d => d.DeclaracionesZAGEP);
    }
}
