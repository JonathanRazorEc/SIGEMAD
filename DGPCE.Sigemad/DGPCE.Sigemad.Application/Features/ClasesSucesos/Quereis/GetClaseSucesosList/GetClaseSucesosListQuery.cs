using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ClasesSucesos.Quereis.GetClaseSucesosList
{
    public class GetClaseSucesosListQuery : IRequest<IReadOnlyList<ClaseSuceso>>
    {
    }
}
