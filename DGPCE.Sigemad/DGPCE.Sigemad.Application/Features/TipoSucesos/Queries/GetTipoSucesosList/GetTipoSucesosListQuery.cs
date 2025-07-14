using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TipoSucesos.Queries.GetTipoSucesosList
{
    public class GetTipoSucesosListQuery: IRequest<IReadOnlyList<TipoSuceso>>
    {
    }
}
