using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.DeleteOpeDatosFronteras
{
    public class DeleteOpeDatoFronteraCommand : IRequest
    {
        public int Id { get; set; }
    }
}
