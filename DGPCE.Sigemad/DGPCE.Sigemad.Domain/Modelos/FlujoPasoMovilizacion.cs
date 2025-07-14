namespace DGPCE.Sigemad.Domain.Modelos;
public class FlujoPasoMovilizacion
{
    public int Id { get; set; }
    public int? IdPasoActual { get; set; }
    public int IdPasoSiguiente { get; set; }
    public int Orden { get; set; }

    public PasoMovilizacion? PasoActual { get; set; }
    public PasoMovilizacion PasoSiguiente { get; set; }
}
