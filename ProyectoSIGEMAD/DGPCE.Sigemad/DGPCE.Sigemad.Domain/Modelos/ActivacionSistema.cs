using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class ActivacionSistema : BaseDomainModel<int>
{
    public int IdRegistro { get; set; }
    public virtual Registro Registro { get; set; } = null!;

    public int IdTipoSistemaEmergencia { get; set; }
    public virtual TipoSistemaEmergencia TipoSistemaEmergencia { get; set; } = null!;

    public DateTime FechaHoraActivacion { get; set; }
    public DateTime? FechaHoraActualizacion { get; set; }

    public string Autoridad { get; set; }
    public string? DescripcionSolicitud { get; set; }
    public string? Observaciones { get; set; }

    public int? IdModoActivacion { get; set; }
    public virtual ModoActivacion ModoActivacion { get; set; } = null!;

    public DateOnly? FechaActivacion { get; set; }
    public string? Codigo { get; set; }

    public string? Nombre { get; set; }

    public string? UrlAcceso { get; set; }

    public DateTime? FechaHoraPeticion { get; set; }

    public DateOnly? FechaAceptacion { get; set; }

    public string? Peticiones { get; set; }

    public string? MediosCapacidades { get; set; }

}
