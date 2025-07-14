namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

public class OpeDatoFronteraIntervaloHorario
{
    public int Id { get; set; }
    public TimeSpan Inicio { get; set; }
    public TimeSpan Fin { get; set; }
    public bool Borrado { get; set; }
}
