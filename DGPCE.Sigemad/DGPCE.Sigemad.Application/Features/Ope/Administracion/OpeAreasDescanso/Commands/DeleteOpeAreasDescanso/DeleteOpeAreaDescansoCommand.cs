using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Commands.DeleteOpeAreasDescanso
{
    public class DeleteOpeAreaDescansoCommand : IRequest
    {
        public int Id { get; set; }
    }
}
