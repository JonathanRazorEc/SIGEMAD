namespace DGPCE.Sigemad.Domain.Modelos;
public class TipoRegistroActualizacion
{
    public int Id { get; set; }
    public string Nombre { get; set; }

    public ICollection<ApartadoRegistro> Apartados { get; set; }
    public ICollection<RegistroActualizacion> Registros { get; set; }
}
