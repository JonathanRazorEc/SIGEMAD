using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesOrganismosTipos.Queries.GetOpeAsistenciasSocialesOrganismosTiposList
{

    public class GetOpeAsistenciasSocialesOrganismosTiposListQuery : IRequest<IReadOnlyList<OpeAsistenciaSocialOrganismoTipo>>
    {
    }
}
