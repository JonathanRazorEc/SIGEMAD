using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
public class ActuacionRelevanteDGPCESpecification : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public ActuacionRelevanteDGPCESpecification(int id)
      : base(d => d.Id == id && d.Borrado == false)
    {
        AddInclude(d => d.EmergenciaNacional);
        AddInclude(d => d.DeclaracionesZAGEP);
        AddInclude(d => d.ConvocatoriasCECOD);
        //AddInclude(d => d.ActivacionPlanEmergencias);
        //AddInclude(d => d.ActivacionSistemas);
        AddInclude(d => d.NotificacionesEmergencias);
        AddInclude("ActivacionPlanEmergencias.TipoPlan");
        AddInclude("ActivacionPlanEmergencias.PlanEmergencia");
        AddInclude("ActivacionPlanEmergencias.Archivo");

        AddInclude(d => d.MovilizacionMedios);
        AddInclude("MovilizacionMedios.Pasos.SolicitudMedio");
        AddInclude("MovilizacionMedios.Pasos.SolicitudMedio.Archivo");
        AddInclude("MovilizacionMedios.Pasos.TramitacionMedio");
        AddInclude("MovilizacionMedios.Pasos.CancelacionMedio");
        AddInclude("MovilizacionMedios.Pasos.OfrecimientoMedio");
        AddInclude("MovilizacionMedios.Pasos.AportacionMedio");
        AddInclude("MovilizacionMedios.Pasos.DespliegueMedio");
        AddInclude("MovilizacionMedios.Pasos.FinIntervencionMedio");
        AddInclude("MovilizacionMedios.Pasos.LlegadaBaseMedio");
    }
}
