using MediatR;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.DeleteSucesosRelacionados;
public class DeleteSucesosRelacionadosCommand: IRequest
{
    public int Id { get; set; }
}
