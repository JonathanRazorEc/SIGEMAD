using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesTipos.Queries.GetOpeAsistenciasSocialesTiposList
{

    public class GetOpeAsistenciasSocialesTiposListQuery : IRequest<IReadOnlyList<OpeAsistenciaSocialTipo>>
    {
    }
}
