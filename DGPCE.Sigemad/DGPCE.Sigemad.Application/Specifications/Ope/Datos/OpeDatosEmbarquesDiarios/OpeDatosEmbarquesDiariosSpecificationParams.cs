namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosEmbarquesDiarios;

public class OpeDatosEmbarquesDiariosSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    //public int? IdOpeLineaMaritima { get; set; }
    public List<int>? IdsOpeLineasMaritimas { get; set; }
    public int? IdOpeFase { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public string? CriterioNumerico { get; set; }
    public string? CriterioNumericoRadio { get; set; }
    public string? CriterioNumericoCriterioCantidad { get; set; }
    public int? CriterioNumericoCriterioCantidadCantidad { get; set; }
    
    /*
    public int? NumeroRotaciones { get; set; }
    public int? NumeroPasajeros { get; set; }
    public int? NumeroTurismos { get; set; }
    public int? NumeroAutocares { get; set; }
    public int? NumeroCamiones { get; set; }
    public int? NumeroTotalVehiculos { get; set; }
    */
}