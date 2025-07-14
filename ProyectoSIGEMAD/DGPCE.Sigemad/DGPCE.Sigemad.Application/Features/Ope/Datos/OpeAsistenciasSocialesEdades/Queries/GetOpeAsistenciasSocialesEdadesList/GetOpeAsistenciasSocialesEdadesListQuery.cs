using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesEdades.Queries.GetOpeAsistenciasSocialesEdadesList
{
    public class GetOpeAsistenciasSocialesEdadesListQuery : IRequest<IReadOnlyList<OpeAsistenciaSocialEdad>>
    {
    }
}
