using DGPCE.Sigemad.Application.Dtos.Registros;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetDetallesRegistrosPorSuceso;
public class GetDetallesRegistrosPorSucesoQuery : SpecificationParams, IRequest<PaginationVm<DetalleRegistroActualizacionDto>>
{
    public int IdSuceso { get; set; }
}
