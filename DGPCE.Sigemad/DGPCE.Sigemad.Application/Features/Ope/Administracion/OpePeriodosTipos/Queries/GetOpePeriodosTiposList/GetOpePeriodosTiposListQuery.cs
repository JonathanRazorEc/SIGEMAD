using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodosTipos.Queries.GetOpePeriodosTiposList
{

    public class GetOpePeriodosTiposListQuery : IRequest<IReadOnlyList<OpePeriodoTipo>>
    {
    }
}
