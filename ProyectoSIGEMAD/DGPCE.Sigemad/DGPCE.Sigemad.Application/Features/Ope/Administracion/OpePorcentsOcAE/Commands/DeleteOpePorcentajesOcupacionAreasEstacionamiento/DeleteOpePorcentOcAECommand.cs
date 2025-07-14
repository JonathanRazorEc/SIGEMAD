using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Commands.DeleteOpePorcentajesOcupacionAreasEstacionamiento
{
    public class DeleteOpePorcentOcAECommand : IRequest
    {
        public int Id { get; set; }
    }
}
