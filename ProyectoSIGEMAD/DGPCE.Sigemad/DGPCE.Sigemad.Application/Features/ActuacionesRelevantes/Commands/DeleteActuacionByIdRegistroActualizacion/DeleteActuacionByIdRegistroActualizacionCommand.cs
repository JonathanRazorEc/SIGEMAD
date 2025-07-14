using MediatR;

namespace DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Commands.DeleteActuacionByIdRegistroActualizacion;
public class DeleteActuacionByIdRegistroActualizacionCommand : IRequest
{
    public int IdRegistroActualizacion { get; set; }
}
