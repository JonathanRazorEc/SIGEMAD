namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
public class AdministracionDto
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public TipoAdministracionDto TipoAdministracion { get; set; }
}
