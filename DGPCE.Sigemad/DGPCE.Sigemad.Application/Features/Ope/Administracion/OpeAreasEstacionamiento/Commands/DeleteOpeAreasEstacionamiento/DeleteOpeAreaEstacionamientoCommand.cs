using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Commands.DeleteOpeAreasEstacionamiento
{
    public class DeleteOpeAreaEstacionamientoCommand : IRequest
    {
        public int Id { get; set; }
    }
}
