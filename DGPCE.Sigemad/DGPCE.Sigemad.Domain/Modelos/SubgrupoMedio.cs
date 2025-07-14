namespace DGPCE.Sigemad.Domain.Modelos;
public class SubgrupoMedio
{
    public int Id { get; set; }
    public string Nombre { get; set; }

    public int IdGrupoMedio { get; set; }
    public GrupoMedio GrupoMedio { get; set; }
}
