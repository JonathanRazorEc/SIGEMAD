
using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
public class ActuacionRelevanteDGPCEWithMovilizacionMedios : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public ActuacionRelevanteDGPCEWithMovilizacionMedios(ActuacionRelevanteDGPCESpecificationParams @params)
     : base(d =>
     (!@params.Id.HasValue || d.Id == @params.Id) &&
     (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
      d.Borrado == false)
    {
        AddInclude(d => d.MovilizacionMedios);
        AddInclude("MovilizacionMedios.Pasos.PasoMovilizacion");
        AddInclude("MovilizacionMedios.Pasos.SolicitudMedio");
        AddInclude("MovilizacionMedios.Pasos.TramitacionMedio");
        AddInclude("MovilizacionMedios.Pasos.CancelacionMedio");
        AddInclude("MovilizacionMedios.Pasos.OfrecimientoMedio");
        AddInclude("MovilizacionMedios.Pasos.AportacionMedio");
        AddInclude("MovilizacionMedios.Pasos.DespliegueMedio");
        AddInclude("MovilizacionMedios.Pasos.FinIntervencionMedio");
        AddInclude("MovilizacionMedios.Pasos.LlegadaBaseMedio");
    }
}
