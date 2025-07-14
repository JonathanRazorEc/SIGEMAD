namespace DGPCE.Sigemad.API.Models.DireccionCoordinacion;

public class DireccionCoordinacionRequest
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }

    public List<ManageDireccionesRequest>? Direcciones { get; set; } = new();

    public List<ManageCoordinacionCecopiRequest>? CoordinacionesCECOPI { get; set; } = new();

    public List<ManageCoordinacionPMARequest>? CoordinacionesPMA { get; set; } = new();
}
