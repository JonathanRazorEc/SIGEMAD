using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFases.Queries.GetOpeFasesList
{

    public class GetOpeFasesListQuery : IRequest<IReadOnlyList<OpeFase>>
    {
    }
}
