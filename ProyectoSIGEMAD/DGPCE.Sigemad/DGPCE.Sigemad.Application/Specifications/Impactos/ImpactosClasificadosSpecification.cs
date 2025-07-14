using Azure.Core;
using DGPCE.Sigemad.Domain;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Impactos;
public class ImpactosClasificadosSpecification : BaseSpecification<ImpactoClasificado>
{
    public ImpactosClasificadosSpecification(ImpactoClasificadoParams request)
    : base(i =>
         (!request.IdTipoImpacto.HasValue ||
         (request.IdTipoImpacto.Value > 0 && i.IdTipoImpacto == request.IdTipoImpacto)) &&
         (string.IsNullOrEmpty(request.busqueda) || i.Descripcion.Contains(request.busqueda)) &&
         (!request.nuclear.HasValue || i.Nuclear == request.nuclear) &&
         (i.Borrado == false))

    {
        AddInclude(i => i.TipoImpacto);
    }
}
