namespace DGPCE.Sigemad.Application.Dtos.Registros;
public class DetalleRegistroActualizacionDto
{
    public int Id { get; set; }
    public int IdRegistro { get; set; }
    public DateTime FechaHora { get; set; }
    public string Ambito { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string Datos { get; set; } = string.Empty;
}
