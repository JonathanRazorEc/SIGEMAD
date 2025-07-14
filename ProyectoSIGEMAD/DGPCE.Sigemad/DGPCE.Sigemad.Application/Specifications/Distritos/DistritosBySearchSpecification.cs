using DGPCE.Sigemad.Application.Specifications.ComunidadesAutonomas;
using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.Distritos;
public class DistritosBySearchSpecification : BaseSpecification<Distrito>
{
    public DistritosBySearchSpecification(DistritosBySearchSpecificationParams request)
        : base(distrito =>
            (string.IsNullOrEmpty(request.busqueda) || distrito.Descripcion.Contains(request.busqueda)) &&
            (!request.IdPais.HasValue || request.IdPais > 0 && distrito.IdPais == request.IdPais))
    {
        AddOrderBy(m => m.Descripcion); // Ordenación por defecto
        AddInclude(m => m.Pais);
    }
}
