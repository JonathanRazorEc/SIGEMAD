namespace DGPCE.Sigemad.Application.Dtos.Sucesos;
public class SucesoGridDto
{
    public int Id { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public string TipoSuceso { get; set; }
    public string Estado { get; set; }
    public string Denominacion { get; set; }
}


