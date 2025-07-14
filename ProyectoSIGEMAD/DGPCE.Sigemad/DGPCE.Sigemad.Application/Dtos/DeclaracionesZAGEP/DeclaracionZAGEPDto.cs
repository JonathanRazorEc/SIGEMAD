using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
public class DeclaracionZAGEPDto
{
    public int Id { get; set; }
    public DateOnly FechaSolicitud { get; set; }
    public string Denominacion { get; set; }
    public string? Observaciones { get; set; } = null;
    public bool EsEliminable { get; set; }

}
