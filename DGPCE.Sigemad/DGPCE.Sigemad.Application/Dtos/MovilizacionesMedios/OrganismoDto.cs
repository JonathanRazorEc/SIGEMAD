namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
public class OrganismoDto
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public AdministracionDto Administracion { get; set; }
}
