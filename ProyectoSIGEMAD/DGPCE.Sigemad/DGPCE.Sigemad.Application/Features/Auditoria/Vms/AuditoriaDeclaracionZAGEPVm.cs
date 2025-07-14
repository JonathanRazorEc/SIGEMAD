
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Features.Auditoria.Vms
{
    public class AuditoriaDeclaracionZAGEPVm
    {
        //Auditoria_DeclaracionZAGEP.[FechaSolicitud] [date] NOT NULL, 
        public DateOnly Fecha { get; set; }

        //Denominacion Auditoria_DeclaracionZAGEP.[Denominacion][nvarchar] (255) NOT
        public string Denominacion { get; set; }

    }

}
