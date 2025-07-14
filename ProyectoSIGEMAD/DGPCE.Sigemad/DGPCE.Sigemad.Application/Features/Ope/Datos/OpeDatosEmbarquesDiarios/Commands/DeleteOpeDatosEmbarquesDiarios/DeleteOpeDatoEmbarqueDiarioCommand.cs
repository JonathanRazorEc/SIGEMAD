using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Commands.DeleteOpeDatosEmbarquesDiarios
{
    public class DeleteOpeDatoEmbarqueDiarioCommand : IRequest
    {
        public int Id { get; set; }
    }
}
