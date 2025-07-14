using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesTareasTipos.Queries.GetOpeAsistenciasSocialesTareasTiposList
{

    public class GetOpeAsistenciasSocialesTareasTiposListQuery : IRequest<IReadOnlyList<OpeAsistenciaSocialTareaTipo>>
    {
    }
}
