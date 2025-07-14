namespace DGPCE.Sigemad.Domain.Modelos;
public class MediosCapacidad
{
    public int Id { get; set; }


    public int IdTipoCapacidad { get; set; }
    public TipoCapacidad TipoCapacidad { get; set; }


    public int IdTipoMedio { get; set; }
    public TipoMedio TipoMedio { get; set; }

    public string Descripcion { get; set; }
    public int NumeroMedio { get; set; }
}
