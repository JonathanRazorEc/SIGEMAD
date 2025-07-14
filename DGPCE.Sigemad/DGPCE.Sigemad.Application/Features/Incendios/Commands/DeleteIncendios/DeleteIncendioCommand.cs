using MediatR;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.DeleteIncendios
{
    public class DeleteIncendioCommand : IRequest
    {
        public int Id { get; set; }
    }
}
