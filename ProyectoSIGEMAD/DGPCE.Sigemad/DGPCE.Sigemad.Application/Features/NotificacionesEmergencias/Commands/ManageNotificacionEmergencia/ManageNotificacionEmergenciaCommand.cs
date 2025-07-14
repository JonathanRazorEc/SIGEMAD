using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
public class ManageNotificacionEmergenciaCommand : IRequest<ManageNotificacionEmergenciaResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }

    public List<ManageNotificacionEmergenciaDto>? Detalles { get; set; } = new();
}
