using DGPCE.Sigemad.Application.Dtos.Impactos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.TipoImpactoEvolucion;
public class ManageTipoImpactoEvolucionDTO
{
    public int? Id { get; set; }
    public int IdTipoImpacto { get; set; }
    public int? Estimado { get; set; }
    public List<ManageImpactoDto> ImpactosEvoluciones { get; set; } = new();
}
