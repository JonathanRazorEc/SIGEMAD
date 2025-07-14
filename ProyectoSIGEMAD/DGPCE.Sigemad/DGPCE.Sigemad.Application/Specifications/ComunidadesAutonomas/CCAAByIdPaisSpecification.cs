using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.ComunidadesAutonomas;
internal class CCAAByIdPaisSpecification : BaseSpecification<Ccaa>
{
    public CCAAByIdPaisSpecification(int idPais)
    {
        
        AddCriteria(c => c.Pais.Id == idPais);
        AddOrderBy(c => c.Descripcion);
    }
}
