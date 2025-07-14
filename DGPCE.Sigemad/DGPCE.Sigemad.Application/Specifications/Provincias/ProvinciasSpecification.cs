using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Specifications.Provincias;

public class ProvinciasSpecification : BaseSpecification<Provincia>
{
    public ProvinciasSpecification(ProvinciaSpecificationParams request)
     : base(Provincia =>
         (!request.Id.HasValue || Provincia.Id == request.Id) &&
        (!request.IdCcaa.HasValue || Provincia.IdCcaa == request.IdCcaa) &&
        (Provincia.Borrado == false))
    {
        AddOrderBy(i => i.Descripcion); // Ordenación por defecto
    }

}
