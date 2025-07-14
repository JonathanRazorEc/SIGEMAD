namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
public class CapacidadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public bool Gestionado { get; set; }

    public TipoCapacidadDto? TipoCapacidad { get; set; }

    public EntidadDto? Entidad { get; set; }
}
