using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesNacionalidades.Queries.GetOpeAsistenciasSocialesNacionalidadesList
{

    public class GetOpeAsistenciasSocialesNacionalidadesListQuery : IRequest<IReadOnlyList<OpeAsistenciaSocialNacionalidad>>
    {
    }
}
