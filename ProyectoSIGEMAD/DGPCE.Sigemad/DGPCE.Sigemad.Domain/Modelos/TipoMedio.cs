namespace DGPCE.Sigemad.Domain.Modelos;
public class TipoMedio
{
    public int Id { get; set; }
    public string Nombre { get; set; }

    public int IdSubgrupoMedio { get; set; }
    public SubgrupoMedio SubgrupoMedio { get; set; }
}
