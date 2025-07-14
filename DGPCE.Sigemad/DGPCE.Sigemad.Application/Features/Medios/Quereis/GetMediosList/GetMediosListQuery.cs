

using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Medios.Quereis.GetMediosList
{
    public class GetMediosListQuery : IRequest<IReadOnlyList<Medio>>
    {
    }
}
