using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Commands.DeleteOpePuntosControlCarreteras
{
    public class DeleteOpePuntoControlCarreteraCommand : IRequest
    {
        public int Id { get; set; }
    }
}
