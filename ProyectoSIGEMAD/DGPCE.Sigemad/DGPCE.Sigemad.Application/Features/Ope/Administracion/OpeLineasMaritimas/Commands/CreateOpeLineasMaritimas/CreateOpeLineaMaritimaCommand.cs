using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.CreateOpeLineasMaritimas;

public class CreateOpeLineaMaritimaCommand : IRequest<CreateOpeLineaMaritimaResponse>
{
    public string Nombre { get; set; } = null!;
    public int IdOpePuertoOrigen { get; set; }
    public int IdOpePuertoDestino { get; set; }
    public int IdOpeFase { get; set; }
    public DateTime FechaValidezDesde { get; set; }
    public DateTime? FechaValidezHasta { get; set; }

    public int? NumeroRotaciones { get; set; }
    public int? NumeroPasajeros { get; set; }
    public int? NumeroTurismos { get; set; }
    public int? NumeroAutocares { get; set; }
    public int? NumeroCamiones { get; set; }
    public int? NumeroTotalVehiculos { get; set; }

}
