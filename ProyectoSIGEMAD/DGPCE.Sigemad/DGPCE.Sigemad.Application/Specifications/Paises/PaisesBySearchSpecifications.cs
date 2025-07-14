using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Paises;
public class PaisesBySearchSpecifications : BaseSpecification<Pais>
{
    public PaisesBySearchSpecifications(PaisesBySearchSpecificationsParams param)
        : base(p => (string.IsNullOrEmpty(param.busqueda) || p.Descripcion.ToLower().Contains(param.busqueda.ToLower())) &&
                    (param.mostrarNacional || p.Id != (int)PaisesEnum.Espana))
    {

            AddOrderBy(p => p.Id == (int)PaisesEnum.Portugal ? 0 :
                             p.Id == (int)PaisesEnum.Francia ? 1 : 2);
            AddOrderBy(p => p.Id != (int)PaisesEnum.Portugal && p.Id != (int)PaisesEnum.Francia ? p.Descripcion : null);
       
  
    }
}