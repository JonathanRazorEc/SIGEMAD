using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;

public class CreateOpeDatoFronteraCommand : IRequest<CreateOpeDatoFronteraResponse>
{
    public int IdOpeFrontera { get; set; }
    public DateTime Fecha { get; set; }
    public int IdOpeDatoFronteraIntervaloHorario { get; set; }
    public bool IntervaloHorarioPersonalizado { get; set; }
    public TimeSpan? InicioIntervaloHorarioPersonalizado { get; set; }
    public TimeSpan? FinIntervaloHorarioPersonalizado { get; set; }
    public int NumeroVehiculos { get; set; }
    public string Afluencia { get; set; } = null!;

}
