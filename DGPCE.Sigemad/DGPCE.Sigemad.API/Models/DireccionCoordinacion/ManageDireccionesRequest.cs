using System.ComponentModel.DataAnnotations;

namespace DGPCE.Sigemad.API.Models.DireccionCoordinacion;

public class ManageDireccionesRequest
{
    public int? Id { get; set; }

    [Required]
    public int IdTipoDireccionEmergencia { get; set; }

    [Required]
    public string AutoridadQueDirige { get; set; }

    [Required]
    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public IFormFile? Archivo { get; set; }

    public bool ActualizarFichero { get; set; }
}
