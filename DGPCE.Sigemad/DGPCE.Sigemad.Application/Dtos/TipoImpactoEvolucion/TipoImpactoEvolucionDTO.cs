using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.TipoImpactoEvolucion;
public class TipoImpactoEvolucionDTO
{
    public int Id { get; set; }
    public int? Estimado { get; set; }

    public int? Total { get; set; }
    public List<ImpactoDto> ImpactosEvoluciones { get; set; } = new();

    public virtual TipoImpacto TipoImpacto { get; set; } 


    public bool EsEliminable { get; set; }

    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    // NUEVOS INDICADORES
    public bool EsNuevo { get; set; }
    public bool EsModificado { get; set; }
}
