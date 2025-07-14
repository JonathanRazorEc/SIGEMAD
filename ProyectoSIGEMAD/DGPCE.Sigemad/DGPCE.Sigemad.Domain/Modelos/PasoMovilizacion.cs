namespace DGPCE.Sigemad.Domain.Modelos;
public class PasoMovilizacion
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int IdEstadoMovilizacion { get; set; }
    public EstadoMovilizacion EstadoMovilizacion { get; set; }
}
