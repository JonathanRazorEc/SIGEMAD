using MediatR;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.DeleteOtrasInformaciones;
public class DeleteDetalleOtraInformacionCommand : IRequest
{    
    public int IdDetalleOtraInformacion { get; set; }
}
