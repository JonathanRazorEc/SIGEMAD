namespace DGPCE.Sigemad.Domain.Modelos;
public class RegistroApartado
{
    public int Id { get; set; }
    public int IdRegistroActualizacion { get; set; }
    public int IdApartadoRegistro { get; set; }

    public RegistroActualizacion RegistroActualizacion { get; set; }
    public ApartadoRegistro ApartadoRegistro { get; set; }
}
