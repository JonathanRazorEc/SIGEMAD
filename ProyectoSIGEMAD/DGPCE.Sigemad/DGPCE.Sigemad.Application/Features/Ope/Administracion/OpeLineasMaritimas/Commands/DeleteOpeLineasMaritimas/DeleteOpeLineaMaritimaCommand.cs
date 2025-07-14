using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.DeleteOpeLineasMaritimas
{
    public class DeleteOpeLineaMaritimaCommand : IRequest
    {
        public int Id { get; set; }
    }
}
