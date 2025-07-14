using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePorcentajesOcupacionAreasEstacionamiento.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePorcentajesOcupacionAreasEstacionamiento;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Queries.GetOpePorcentajesOcupacionAreasEstacionamientoList
{

    public class GetOpePorcentsOcAEListQuery : OpePorcentajesOcupacionAreasEstacionamientoSpecificationParams, IRequest<PaginationVm<OpePorcentajeOcupacionAreaEstacionamientoVm>>
    {
    }
}
