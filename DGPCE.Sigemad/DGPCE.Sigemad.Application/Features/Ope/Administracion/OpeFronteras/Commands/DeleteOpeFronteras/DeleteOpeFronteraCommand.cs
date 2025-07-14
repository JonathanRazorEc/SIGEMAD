using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.DeleteOpeFronteras
{
    public class DeleteOpeFronteraCommand : IRequest
    {
        public int Id { get; set; }
    }
}
