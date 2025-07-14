using MediatR;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.DeleteSucesoRelacionadoPorRegistro;
public class DeleteSucesoRelacionadoPorRegistroCommand : IRequest
{
    public int IdRegistroActualizacion { get; set; }
}
