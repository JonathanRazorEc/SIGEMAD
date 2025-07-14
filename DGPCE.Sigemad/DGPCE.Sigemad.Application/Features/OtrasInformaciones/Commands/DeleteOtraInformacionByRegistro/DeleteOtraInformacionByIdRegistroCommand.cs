using MediatR;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.DeleteOtraInformacionByRegistro;
public class DeleteOtraInformacionByIdRegistroCommand : IRequest
{
    public int IdRegistroActualizacion { get; set; }
}
