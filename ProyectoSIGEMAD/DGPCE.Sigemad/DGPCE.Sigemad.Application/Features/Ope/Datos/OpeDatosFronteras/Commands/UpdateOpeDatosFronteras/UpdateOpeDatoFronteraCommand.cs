using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.UpdateOpeDatosFronteras;

public class UpdateOpeDatoFronteraCommand : IRequest
{
    public int Id { get; set; }
    public int IdOpeFrontera { get; set; }
    public DateTime Fecha { get; set; }
    public int IdOpeDatoFronteraIntervaloHorario { get; set; }
    public bool IntervaloHorarioPersonalizado { get; set; }
    public TimeSpan? InicioIntervaloHorarioPersonalizado { get; set; }
    public TimeSpan? FinIntervaloHorarioPersonalizado { get; set; }
    public int NumeroVehiculos { get; set; }
    public string Afluencia { get; set; } = null!;
}
