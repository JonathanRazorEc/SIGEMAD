using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.CreateOpeFronteras;

public class CreateOpeFronteraCommand : IRequest<CreateOpeFronteraResponse>
{
    public string Nombre { get; set; } = null!;
    public int IdCcaa { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Carretera { get; set; } = null!;
    public decimal PK { get; set; }
    public int CoordenadaUTM_X { get; set; }
    public int CoordenadaUTM_Y { get; set; }
    public int? TransitoMedioVehiculos { get; set; }
    public int? TransitoAltoVehiculos { get; set; }

}
