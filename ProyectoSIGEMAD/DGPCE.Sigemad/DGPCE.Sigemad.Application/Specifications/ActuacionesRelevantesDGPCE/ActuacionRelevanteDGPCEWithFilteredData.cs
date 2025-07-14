using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
public class ActuacionRelevanteDGPCEWithFilteredData : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public ActuacionRelevanteDGPCEWithFilteredData(int id, List<int> idsActivacionPlanEmergencias, List<int> idsDeclaracionesZAGEP, List<int> idsActivacionSistemas,
        List<int> idsConvocatoriasCECOD, List<int> idsNotificacionesEmergencias, List<int> idsMovilizacionMedios, bool isEmergenciaNacional = false)
       : base(d => d.Id == id && d.Borrado == false)
    {
        if (isEmergenciaNacional)
        {
            AddInclude(d => d.EmergenciaNacional);
        }

        if (idsDeclaracionesZAGEP.Any())
        {
            AddInclude(d => d.DeclaracionesZAGEP.Where(declaracionZagep => idsDeclaracionesZAGEP.Contains(declaracionZagep.Id) && !declaracionZagep.Borrado));
        }


        if (idsConvocatoriasCECOD.Any())
        {
            AddInclude(d => d.ConvocatoriasCECOD.Where(convocatoriasCECOD => idsConvocatoriasCECOD.Contains(convocatoriasCECOD.Id) && !convocatoriasCECOD.Borrado));
        }

        if (idsNotificacionesEmergencias.Any())
        {
            AddInclude(d => d.NotificacionesEmergencias.Where(notificacionEmergencia => idsNotificacionesEmergencias.Contains(notificacionEmergencia.Id) && !notificacionEmergencia.Borrado));
            AddInclude("NotificacionesEmergencias.TipoNotificacion");
        }

        if (idsMovilizacionMedios.Any())
        {
            AddInclude(d => d.MovilizacionMedios.Where(movilizacionMedios => idsMovilizacionMedios.Contains(movilizacionMedios.Id) && !movilizacionMedios.Borrado));

            AddInclude("MovilizacionMedios.Pasos.PasoMovilizacion");
            AddInclude("MovilizacionMedios.Pasos.SolicitudMedio");
            AddInclude("MovilizacionMedios.Pasos.SolicitudMedio.ProcedenciaMedio");
            AddInclude("MovilizacionMedios.Pasos.SolicitudMedio.Archivo");

            AddInclude("MovilizacionMedios.Pasos.TramitacionMedio");
            AddInclude("MovilizacionMedios.Pasos.TramitacionMedio.DestinoMedio");

            AddInclude("MovilizacionMedios.Pasos.CancelacionMedio");
            AddInclude("MovilizacionMedios.Pasos.OfrecimientoMedio");

            AddInclude("MovilizacionMedios.Pasos.AportacionMedio");
            AddInclude("MovilizacionMedios.Pasos.AportacionMedio.Capacidad");
            AddInclude("MovilizacionMedios.Pasos.AportacionMedio.TipoAdministracion");

            AddInclude("MovilizacionMedios.Pasos.DespliegueMedio");
            AddInclude("MovilizacionMedios.Pasos.DespliegueMedio.Capacidad");

            AddInclude("MovilizacionMedios.Pasos.FinIntervencionMedio");
            AddInclude("MovilizacionMedios.Pasos.FinIntervencionMedio.Capacidad");

            AddInclude("MovilizacionMedios.Pasos.LlegadaBaseMedio");
            AddInclude("MovilizacionMedios.Pasos.LlegadaBaseMedio.Capacidad");
        }
    }
}
