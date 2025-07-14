namespace DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

public class OpePais
{
    public int Id { get; set; }
    public int IdPais { get; set; }
    public bool Extranjero { get; set; }
    public bool OpePuertos { get; set; }
    public bool OpeDatosAsistencias { get; set; }
    public string? RutaImagen { get; set; }
    public bool Borrado { get; set; }

    public virtual Pais Pais { get; set; } = null!;
}
