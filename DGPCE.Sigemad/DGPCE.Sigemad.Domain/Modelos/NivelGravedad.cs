namespace DGPCE.Sigemad.Domain.Modelos;

public class NivelGravedad
{
    public NivelGravedad() { }

    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public int Gravedad { get; set; }

    public int RiesgoAemet { get; set; }

    public bool? EsNivelDeParte { get; set; }

    public int? OrdenAemet { get; set; }

    public int? OrdenParte { get; set; }
}
