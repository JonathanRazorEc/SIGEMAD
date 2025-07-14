using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
public class ActivacionSistemaDto
{
    public int Id { get; set; }
    public int IdTipoSistemaEmergencia { get; set; }

    public DateTime? FechaHoraActivacion { get; set; }
    public DateTime? FechaHoraActualizacion { get; set; }
    public string? Autoridad { get; set; }
    public string? DescripcionSolicitud { get; set; }
    public string? Observaciones { get; set; }
    public int? IdModoActivacion { get; set; }
    public DateOnly? FechaActivacion { get; set; }
    public string? Codigo { get; set; }

    public string? Nombre { get; set; }

    public string? UrlAcceso { get; set; }

    public DateTime? FechaHoraPeticion { get; set; }

    public DateOnly? FechaAceptacion { get; set; }

    public string? Peticiones { get; set; }

    public string? MediosCapacidades { get; set; }

    public bool EsEliminable { get; set; }

}
