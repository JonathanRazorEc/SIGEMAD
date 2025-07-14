namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
public class EntidadDto
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public OrganismoDto Organismo { get; set; }
    public int IdOrganismo { get; set; }
}
