using DGPCE.Sigemad.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos;
public class TipoImpactoEvolucion : BaseDomainModel<int>
{
    public int IdRegistro { get; set; }

    public int IdTipoImpacto { get; set; }
    public int? Estimado { get; set; }

    public virtual Registro Registro { get; set; }
    public virtual TipoImpacto TipoImpacto { get; set; }

    public virtual List<ImpactoEvolucion>  ImpactosEvoluciones { get; set; }

}
