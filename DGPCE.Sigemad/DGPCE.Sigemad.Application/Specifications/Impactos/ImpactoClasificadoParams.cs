using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Impactos;
public class ImpactoClasificadoParams:SpecificationParams
{
    public int? IdTipoImpacto { get; set; }
    public bool? nuclear { get; set; } = false;  
    public string? busqueda { get; set; } =null;
}
