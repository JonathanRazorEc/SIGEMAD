using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Commands.DeleteOpePuertos
{
    public class DeleteOpePuertoCommand : IRequest
    {
        public int Id { get; set; }
    }
}
