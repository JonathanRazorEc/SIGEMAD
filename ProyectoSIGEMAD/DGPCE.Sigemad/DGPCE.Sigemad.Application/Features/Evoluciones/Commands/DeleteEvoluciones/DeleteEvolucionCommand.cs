using MediatR;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvoluciones;
public class DeleteEvolucionCommand : IRequest
{
    public int Id { get; set; }
}
