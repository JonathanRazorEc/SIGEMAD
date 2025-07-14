using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescansoTipos.Queries.GetOpeAreasDescansoTiposList
{

    public class GetOpeAreasDescansoTiposListQuery : IRequest<IReadOnlyList<OpeAreaDescansoTipo>>
    {
    }
}
