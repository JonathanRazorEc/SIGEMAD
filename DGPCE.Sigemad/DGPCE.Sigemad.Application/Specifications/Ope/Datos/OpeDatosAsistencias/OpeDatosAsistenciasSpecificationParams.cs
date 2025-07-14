namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosAsistencias;

public class OpeDatosAsistenciasSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    //public int? IdOpePuerto { get; set; }
    public List<int>? IdsOpePuertos { get; set; }
    public int? IdOpeFase { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public string? CriterioNumerico { get; set; }
    public string? CriterioNumericoRadio { get; set; }
    public string? CriterioNumericoCriterioCantidad { get; set; }
    public int? CriterioNumericoCriterioCantidadCantidad { get; set; }
    
    /*
    public int? Sanitarias { get; set; }
    public int? Sociales { get; set; }
    public int? Traducciones { get; set; }
    public int? NumeroAutocares { get; set; }
    public int? NumeroCamiones { get; set; }
    public int? NumeroTotalVehiculos { get; set; }
    */

}