using System.ComponentModel.DataAnnotations;

namespace DGPCE.Sigemad.API.Models;

public class DetalleDocumentacionRequest
{
    public int? Id { get; set; }

    [Required]
    public DateTime FechaHora { get; set; }

    [Required]
    public DateTime FechaHoraSolicitud { get; set; }

    [Required]
    public int IdTipoDocumento { get; set; }

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    public IFormFile? Archivo { get; set; }

    public List<int>? IdsProcedenciasDestinos { get; set; } = new();
}