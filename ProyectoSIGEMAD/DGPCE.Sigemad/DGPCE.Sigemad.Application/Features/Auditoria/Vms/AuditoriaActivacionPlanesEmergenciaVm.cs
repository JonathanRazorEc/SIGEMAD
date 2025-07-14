
namespace DGPCE.Sigemad.Application.Features.Auditoria.Vms
{
    public class AuditoriaActivacionPlanesEmergenciaVm
    {
        //Auditoria_ActivacionPlanesEmergencia.[IdTipoPlan]
        public string? TipoPlan { get; set; }

        //PlanEnmergencia.[Descripcion]
        public string? PlanEmergencia { get; set; }

        //Auditoria_ActivacionPlanesEmergencia.[FechaInicio]
        public DateOnly FechaInicio { get; set; }

        //Auditoria_ActivacionPlanesEmergencia.[FechaFin]
        public DateOnly? FechaFinal { get; set; }
        
        //Auditoria_ActivacionPlanesEmergencia.[Autoridad]
        public string Autoridad { get; set; }


    }

}
