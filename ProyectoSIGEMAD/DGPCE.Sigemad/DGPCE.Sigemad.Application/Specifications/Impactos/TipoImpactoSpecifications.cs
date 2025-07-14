using DGPCE.Sigemad.Domain;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Impactos;
public class TipoImpactoSpecifications : BaseSpecification<ImpactoClasificado>
{
    public TipoImpactoSpecifications(bool nuclear)
  : base(i => i.Borrado == false && i.Nuclear == nuclear)
    {

        AddInclude(i => i.TipoImpacto);
    }
}
