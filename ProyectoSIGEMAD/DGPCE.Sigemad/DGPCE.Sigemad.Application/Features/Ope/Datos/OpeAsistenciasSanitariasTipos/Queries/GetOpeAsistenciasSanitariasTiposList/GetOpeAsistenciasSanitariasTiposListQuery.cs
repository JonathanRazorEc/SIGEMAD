using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSanitariasTipos.Queries.GetOpeAsistenciasSanitariasTiposList
{

    public class GetOpeAsistenciasSanitariasTiposListQuery : IRequest<IReadOnlyList<OpeAsistenciaSanitariaTipo>>
    {
    }
}
