namespace DGPCE.Sigemad.Domain.Modelos;
public class ApartadoRegistro
{
    public int Id { get; set; }
    public int IdTipoRegistroActualizacion { get; set; }
    public string Nombre { get; set; }
    public int Orden { get; set; }

    public TipoRegistroActualizacion TipoRegistroActualizacion { get; set; }
    public ICollection<RegistroApartado> RegistrosApartados { get; set; }
    public ICollection<DetalleRegistroActualizacion> DetallesRegistro { get; set; }
}

