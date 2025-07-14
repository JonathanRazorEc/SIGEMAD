using MediatR;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvolucionesByIdRegistro;
public class DeleteEvolucionByIdRegistroCommand: IRequest
{
    public int IdRegistroActualizacion { get; set; }
}
