using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Vms;
public class SucesosBusquedaVm
{
    public int IdSuceso { get; set; } 
    public DateTime FechaHora { get; set; }
    public string TipoSuceso { get; set; }
    public string? Estado { get; set; }
    public string Denominacion { get; set; }

}
