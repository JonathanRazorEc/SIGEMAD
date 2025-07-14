using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.MunicipiosExtranjeros;
public class MunicipiosExtranjerosBySearchSpecification : BaseSpecification<MunicipioExtranjero>
{
    public MunicipiosExtranjerosBySearchSpecification(MunicipiosExtranjerosBySearchSpecificationParams request)
        : base(municipio =>
            (string.IsNullOrEmpty(request.busqueda) || municipio.Descripcion.Contains(request.busqueda)) &&
            (!request.IdDistrito.HasValue || request.IdDistrito > 0 && municipio.IdDistrito == request.IdDistrito) &&
            (!request.IdPais.HasValue || request.IdPais > 0 && municipio.Distrito.IdPais == request.IdPais) &&
            (municipio.Borrado == false))
    {
        AddOrderBy(m => m.Descripcion); // Ordenación por defecto
        AddInclude(m => m.Distrito);
        AddInclude(m => m.Distrito.Pais);    
    }
}