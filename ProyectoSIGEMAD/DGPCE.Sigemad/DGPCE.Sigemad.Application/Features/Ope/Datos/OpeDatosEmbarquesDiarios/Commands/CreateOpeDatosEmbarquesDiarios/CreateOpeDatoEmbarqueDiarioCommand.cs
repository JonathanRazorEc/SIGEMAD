using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Commands.CreateOpeDatosEmbarquesDiarios;

public class CreateOpeDatoEmbarqueDiarioCommand : IRequest<CreateOpeDatoEmbarqueDiarioResponse>
{
    public int IdOpeLineaMaritima { get; set; }
    public DateTime Fecha { get; set; }
    public int NumeroRotaciones { get; set; }
    public int NumeroPasajeros { get; set; }
    public int NumeroTurismos { get; set; }
    public int NumeroAutocares { get; set; }
    public int NumeroCamiones { get; set; }
    public int NumeroTotalVehiculos { get; set; }
}
