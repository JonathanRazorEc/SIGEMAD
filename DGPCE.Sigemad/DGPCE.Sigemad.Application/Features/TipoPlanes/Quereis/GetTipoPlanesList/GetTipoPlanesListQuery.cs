using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TipoPlanes.Quereis.GetTipoPlanesList;
public class GetTipoPlanesListQuery : IRequest<IReadOnlyList<TipoPlan>>
{
    public int IdAmbito { get; set; }
    public int IdTipoSuceso { get; set; }
    public GetTipoPlanesListQuery(int IdAmbito, int IdTipoSuceso)
    {
        this.IdAmbito = IdAmbito;
        this.IdTipoSuceso = IdTipoSuceso;
    }
}
