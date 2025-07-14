using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePaises.Queries.GetOpePaisesList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePaises;

public class OpePaisesSpecification : BaseSpecification<OpePais>
{
    public OpePaisesSpecification(GetOpePaisesListQuery request)
    {
        if (request.Extranjero.HasValue)
            AddCriteria(p => p.Extranjero == request.Extranjero.Value);

        if (request.OpePuertos.HasValue)
            AddCriteria(p => p.OpePuertos == request.OpePuertos.Value);

        if (request.OpeDatosAsistencias.HasValue)
            AddCriteria(p => p.OpeDatosAsistencias == request.OpeDatosAsistencias.Value);

        // Siempre quieres excluir los borrados
        AddCriteria(p => p.Borrado != true);

        AddInclude(i => i.Pais);

        AddOrderBy(i => i.Pais.Descripcion);
    }
}
