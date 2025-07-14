using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronterasIntervalosHorarios.Queries.GetOpeDatosFronterasIntervalosHorariosList
{
    public class GetOpeDatosFronterasIntervalosHorariosListQuery : IRequest<IReadOnlyList<OpeDatoFronteraIntervaloHorario>>
    {
    }
}
