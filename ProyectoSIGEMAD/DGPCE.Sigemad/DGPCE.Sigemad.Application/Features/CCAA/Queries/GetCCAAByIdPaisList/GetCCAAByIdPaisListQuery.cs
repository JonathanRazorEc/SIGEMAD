using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CCAA.Queries.GetCCAAByIdPaisList;
public class GetCCAAByIdPaisListQuery : IRequest<IReadOnlyList<ComunidadesAutonomasSinProvinciasVm>>
{
    public int IdPais { get; set; }

    public GetCCAAByIdPaisListQuery(int idPais)
    {
        IdPais = idPais;
    }
}
