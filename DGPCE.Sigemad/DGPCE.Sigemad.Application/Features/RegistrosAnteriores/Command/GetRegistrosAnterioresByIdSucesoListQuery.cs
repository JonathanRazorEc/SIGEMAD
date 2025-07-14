
using DGPCE.Sigemad.Application.Dtos.RegistrosAnteriores;
using DGPCE.Sigemad.Application.Specifications;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.RegistrosAnteriores.Command;
public class GetRegistrosAnterioresByIdSucesoListQuery : IRequest<IReadOnlyList<RegistrosAnterioresDto>>
{
    public int IdSuceso { get; set; }

    public int? IdRegistro { get; set; }
}
