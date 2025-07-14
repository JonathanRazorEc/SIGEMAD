using System.Text.Json;
using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

public class OpePuerto : BaseDomainModel<int>
{
    public string Nombre { get; set; } = null!;
    public int IdOpeFase { get; set; }
    public int IdPais { get; set; }
    public int? IdCcaa { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }
    public int CoordenadaUTM_X { get; set; }
    public int CoordenadaUTM_Y { get; set; }
    public DateTime FechaValidezDesde { get; set; }
    public DateTime? FechaValidezHasta { get; set; }
    public int? Capacidad { get; set; }

    public virtual OpeFase OpeFase { get; set; } = null!;
}
