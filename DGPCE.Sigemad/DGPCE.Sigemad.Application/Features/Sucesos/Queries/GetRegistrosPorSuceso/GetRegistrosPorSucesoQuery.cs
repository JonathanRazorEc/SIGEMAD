using DGPCE.Sigemad.Application.Dtos.Registros;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetRegistrosPorSuceso;
public class GetRegistrosPorSucesoQuery : SpecificationParams, IRequest<PaginationVm<RegistroActualizacionDto>>
{
    public int IdSuceso { get; set; }
}
