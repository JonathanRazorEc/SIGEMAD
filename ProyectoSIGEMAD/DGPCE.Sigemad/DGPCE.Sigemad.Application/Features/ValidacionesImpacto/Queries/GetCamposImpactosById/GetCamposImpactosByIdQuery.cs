using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Queries.GetCamposImpactosById;
public class GetCamposImpactosByIdQuery : IRequest<IReadOnlyList<ValidacionImpactoClasificadoVm>>
{
    public int idImpactoClasificado { get; set; }
    public GetCamposImpactosByIdQuery(int id)
    {
        idImpactoClasificado = id;
    }
}
