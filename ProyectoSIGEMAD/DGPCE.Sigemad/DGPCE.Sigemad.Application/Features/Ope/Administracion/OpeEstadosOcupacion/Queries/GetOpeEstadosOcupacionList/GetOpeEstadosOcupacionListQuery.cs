using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeEstadosOcupacion.Queries.GetOpeEstadosOcupacionList
{

    public class GetOpeEstadosOcupacionListQuery : IRequest<IReadOnlyList<OpeEstadoOcupacion>>
    {
    }
}
