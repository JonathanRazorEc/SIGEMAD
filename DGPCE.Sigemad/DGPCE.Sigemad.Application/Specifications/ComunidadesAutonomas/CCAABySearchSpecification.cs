using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.ComunidadesAutonomas;
public class CCAABySearchSpecification : BaseSpecification<Ccaa>
{
    public CCAABySearchSpecification(CCAABySearchSpecificationParams request)
        : base(ccaa =>
            (string.IsNullOrEmpty(request.busqueda) || ccaa.Descripcion.Contains(request.busqueda)) &&
            (!request.IdPais.HasValue || request.IdPais > 0 && ccaa.IdPais == request.IdPais))
    {
        AddOrderBy(m => m.Descripcion); // Ordenación por defecto
        AddInclude(m => m.Pais);
    }
}