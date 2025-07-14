namespace DGPCE.Sigemad.Domain.Modelos;
public class SuperficieFiltro
{
    public int Id { get; set; }
    public string Descripcion { get; set; }

    public int IdTipoFiltro { get; set; }

    public int Valor { get; set; }

    public bool Borrado { get; set; }

    public bool Editable { get; set; }

    public TipoFiltro TipoFiltro { get; set; }
}
