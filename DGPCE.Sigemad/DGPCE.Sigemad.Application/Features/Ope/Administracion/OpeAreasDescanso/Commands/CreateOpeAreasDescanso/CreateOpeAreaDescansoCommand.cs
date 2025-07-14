using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Commands.CreateOpeAreasDescanso;

public class CreateOpeAreaDescansoCommand : IRequest<CreateOpeAreaDescansoResponse>
{
    public string Nombre { get; set; } = null!;
    public int IdOpeAreaDescansoTipo { get; set; }
    public int IdCcaa { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Carretera { get; set; } = null!;
    public decimal PK { get; set; }
    public int CoordenadaUTM_X { get; set; }
    public int CoordenadaUTM_Y { get; set; }
    public int? Capacidad { get; set; }
    //public int IdOpeEstadoOcupacion { get; set; }
}
