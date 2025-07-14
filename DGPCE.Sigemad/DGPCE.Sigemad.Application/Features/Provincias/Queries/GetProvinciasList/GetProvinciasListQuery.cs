using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasList
{
    public class GetProvinciasListQuery : IRequest<IReadOnlyList<ProvinciaSinMunicipiosConIdComunidadVm>>
    {
    }
}
