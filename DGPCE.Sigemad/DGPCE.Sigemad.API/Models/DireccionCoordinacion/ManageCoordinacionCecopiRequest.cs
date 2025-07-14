using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace DGPCE.Sigemad.API.Models.DireccionCoordinacion;

public class ManageCoordinacionCecopiRequest
{

    public int? Id { get; set; }

    [Required]
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    [Required]
    public int IdProvincia { get; set; }

    [Required]
    public int IdMunicipio { get; set; }

    [Required]
    public string Lugar { get; set; } = string.Empty;


    public string? Observaciones { get; set; }

    public IFormFile? Archivo { get; set; }

    public string? GeoPosicion { get; set; } //se asigna tipo objeto para traer al json 

    public bool ActualizarFichero { get; set; }
}
