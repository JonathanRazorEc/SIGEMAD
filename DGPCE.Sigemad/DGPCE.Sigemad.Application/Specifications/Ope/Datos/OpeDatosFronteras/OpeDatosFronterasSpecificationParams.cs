namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;

public class OpeDatosFronterasSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    //public int? IdOpeFrontera { get; set; }
    public List<int>? IdsOpeFronteras { get; set; }

    //public DateOnly? Fecha { get; set; }

    //
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public string? CriterioNumerico { get; set; }
    public string? CriterioNumericoRadio { get; set; }
    public string? CriterioNumericoCriterioCantidad { get; set; }
    public int? CriterioNumericoCriterioCantidadCantidad { get; set; }
    //
    public int? IdOpeOpeDatoFronteraIntervaloHorario { get; set; }
    public bool IntervaloHorarioPersonalizado { get; set; }
    public TimeSpan? InicioIntervaloHorarioPersonalizado { get; set; }
    public TimeSpan? FinIntervaloHorarioPersonalizado { get; set; }
    public int? NumeroVehiculos { get; set; }
    public string? Afluencia { get; set; }

}