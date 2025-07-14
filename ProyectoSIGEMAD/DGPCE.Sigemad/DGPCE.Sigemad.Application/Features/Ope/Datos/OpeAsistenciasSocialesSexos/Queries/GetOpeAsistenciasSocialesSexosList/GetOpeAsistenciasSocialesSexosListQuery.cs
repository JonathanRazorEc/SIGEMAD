using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesSexos.Queries.GetOpeAsistenciasSocialesSexosList
{

    public class GetOpeAsistenciasSocialesSexosListQuery : IRequest<IReadOnlyList<OpeAsistenciaSocialSexo>>
    {
    }
}
