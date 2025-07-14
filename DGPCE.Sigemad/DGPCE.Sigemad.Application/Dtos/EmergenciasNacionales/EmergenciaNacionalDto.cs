using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
public class EmergenciaNacionalDto
{
    public string Autoridad { get; set; }
    public string DescripcionSolicitud { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public DateTime? FechaHoraDeclaracion { get; set; }
    public string? DescripcionDeclaracion { get; set; }
    public DateTime? FechaHoraDireccion { get; set; }
    public string? Observaciones { get; set; }
}
