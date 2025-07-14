using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Commands.UpdateOpeDatosEmbarquesDiarios;

public class UpdateOpeDatoEmbarqueDiarioCommand : IRequest<OpeDatoEmbarqueDiarioVm>
{
    public int Id { get; set; }
    public int IdOpeLineaMaritima{ get; set; }
    public DateTime Fecha { get; set; }
    public int NumeroRotaciones { get; set; }
    public int NumeroPasajeros { get; set; }
    public int NumeroTurismos { get; set; }
    public int NumeroAutocares { get; set; }
    public int NumeroCamiones { get; set; }
    public int NumeroTotalVehiculos { get; set; }
}
