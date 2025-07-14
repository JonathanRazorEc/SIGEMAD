using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.Provincias;
public class ProvinciasBySearchSpecification : BaseSpecification<Provincia>
{
    public ProvinciasBySearchSpecification(ProvinciaSpecificationParams request)
        : base(provincia =>
            (string.IsNullOrEmpty(request.busqueda) || provincia.Descripcion.Contains(request.busqueda)) &&
            (!request.IdCcaa.HasValue || request.IdCcaa > 0 && provincia.IdCcaa == request.IdCcaa) &&
            (provincia.Borrado == false))
    {
        AddOrderBy(m => m.Descripcion); // Ordenación por defecto
        AddInclude(m => m.IdCcaaNavigation);
        AddInclude(m => m.IdCcaaNavigation.Pais);
    }
}