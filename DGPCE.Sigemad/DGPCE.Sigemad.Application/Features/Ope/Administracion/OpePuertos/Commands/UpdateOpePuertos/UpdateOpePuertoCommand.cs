using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.UpdateOpePuertos;

public class UpdateOpePuertoCommand : IRequest
{
    public int Id { get; set; }

    public string Nombre { get; set; }
    public int IdOpeFase { get; set; }
    public int IdPais { get; set; }
    public int? IdCcaa { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }
    public int CoordenadaUTM_Y { get; set; }
    public int CoordenadaUTM_X { get; set; }
    public DateTime FechaValidezDesde { get; set; }
    public DateTime? FechaValidezHasta { get; set; }
    public int? Capacidad { get; set; }

}
