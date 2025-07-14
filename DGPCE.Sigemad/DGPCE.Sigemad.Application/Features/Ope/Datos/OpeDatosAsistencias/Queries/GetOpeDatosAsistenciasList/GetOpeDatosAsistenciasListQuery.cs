using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosAsistencias;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Queries.GetOpeDatosAsistenciasList
{
    public class GetOpeDatosAsistenciasListQuery : OpeDatosAsistenciasSpecificationParams, IRequest<PaginationVm<OpeDatoAsistenciaVm>>
    {
    }
}
