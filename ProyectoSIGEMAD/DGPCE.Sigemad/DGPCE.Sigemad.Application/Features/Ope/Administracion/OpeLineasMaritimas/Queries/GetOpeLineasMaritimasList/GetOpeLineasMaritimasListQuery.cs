using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Queries.GetOpeLineasMaritimasList
{

    public class GetOpeLineasMaritimasListQuery : OpeLineasMaritimasSpecificationParams, IRequest<PaginationVm<OpeLineaMaritimaVm>>
    {
    }
}
