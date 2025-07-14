using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Municipios;

public class MunicipiosSpecification : BaseSpecification<Municipio>
{
    public MunicipiosSpecification(MunicipiosSpecificationParams request)
     : base(municipio =>
         (!request.Id.HasValue || municipio.Id == request.Id) &&
        (!request.IdProvincia.HasValue || municipio.IdProvincia == request.IdProvincia) &&
        (municipio.Borrado == false))
    {
        AddOrderBy(i => i.Descripcion); // Ordenación por defecto
    }

}
