using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.ValidacionImpactosClasificados;
public class ValidacionImpactosClasificadosSpecifications : BaseSpecification<ValidacionImpactoClasificado>
{
     public ValidacionImpactosClasificadosSpecifications(int idImpactoClasificado)
      : base(i => i.IdImpactoClasificado == idImpactoClasificado && i.Borrado == false)
    {
    }
}
