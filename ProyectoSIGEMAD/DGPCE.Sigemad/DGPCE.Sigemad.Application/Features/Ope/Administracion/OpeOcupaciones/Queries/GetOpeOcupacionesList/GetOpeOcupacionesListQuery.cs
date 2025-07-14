using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeOcupaciones.Queries.GetOpeOcupacionesList
{

    public class GetOpeOcupacionesListQuery : IRequest<IReadOnlyList<OpeOcupacion>>
    {
    }
}
