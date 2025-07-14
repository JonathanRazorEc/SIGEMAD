using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
public class ConvocatoriaCECODDto
{
    public int Id { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public string Lugar { get; set; }
    public string Convocados { get; set; }
    public string? Participantes { get; set; }
    public string? Observaciones { get; set; }

    public bool EsEliminable { get; set; }

}
