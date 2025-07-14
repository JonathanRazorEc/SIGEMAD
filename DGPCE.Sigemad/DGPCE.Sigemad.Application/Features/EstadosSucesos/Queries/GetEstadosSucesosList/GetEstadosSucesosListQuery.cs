using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.EstadosSucesos.Queries.GetEstadosSucesosList
{
    public class GetEstadosSucesosListQuery : IRequest<IReadOnlyList<EstadoSuceso>>
    {
    }
}
