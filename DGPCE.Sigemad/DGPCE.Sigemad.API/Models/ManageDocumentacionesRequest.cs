namespace DGPCE.Sigemad.API.Models;

public class ManageDocumentacionesRequest
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<DetalleDocumentacionRequest>? Detalles { get; set; } = new();
}