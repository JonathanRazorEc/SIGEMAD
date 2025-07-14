using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Municipios;
public class MunicipiosBySearchSpecification : BaseSpecification<Municipio>
{
    public MunicipiosBySearchSpecification(MunicipiosSpecificationParams request)
        : base(municipio =>
            (string.IsNullOrEmpty(request.busqueda) || municipio.Descripcion.Contains(request.busqueda)) &&
            (!request.IdProvincia.HasValue || request.IdProvincia > 0 && municipio.IdProvincia == request.IdProvincia) &&
            (municipio.Borrado == false))
    {
        AddOrderBy(m => m.Descripcion); // Ordenación por defecto
        AddInclude(m => m.Provincia);
        AddInclude(m => m.Provincia.IdCcaaNavigation);
        AddInclude(m => m.Provincia.IdCcaaNavigation.Pais);
    }
}