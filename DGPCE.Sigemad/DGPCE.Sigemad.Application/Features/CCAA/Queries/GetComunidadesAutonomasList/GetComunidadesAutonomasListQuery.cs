using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CCAA.Queries.GetComunidadesAutonomasList
{
    public class GetComunidadesAutonomasListQuery : IRequest<IReadOnlyList<ComunidadesAutonomasVm>>
    {

    }
}
