using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Commands.CreateOpeAreasEstacionamiento;

public class CreateOpeAreaEstacionamientoCommand : IRequest<CreateOpeAreaEstacionamientoResponse>
{
    public string Nombre { get; set; } = null!;
    public int IdCcaa { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Carretera { get; set; } = null!;
    public decimal PK { get; set; }
    public int CoordenadaUTM_X { get; set; }
    public int CoordenadaUTM_Y { get; set; }
    public bool InstalacionPortuaria { get; set; }
    public int? IdOpePuerto { get; set; }
    public int Capacidad { get; set; }

}
