namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;

public class OpeLineasMaritimasSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    public string? Nombre { get; set; } = null!;
    public int? IdOpePuertoOrigen { get; set; }
    public int? IdOpePuertoDestino { get; set; }
    public int? IdPaisOrigen { get; set; }
    public int? IdPaisDestino { get; set; }
    public int? IdOpeFase { get; set; }
    public DateOnly? FechaValidezDesde { get; set; }
    public DateOnly? FechaValidezHasta { get; set; }

    public int? NumeroRotaciones { get; set; }
    public int? NumeroPasajeros { get; set; }
    public int? NumeroTurismos { get; set; }
    public int? NumeroAutocares { get; set; }
    public int? NumeroCamiones { get; set; }
    public int? NumeroTotalVehiculos { get; set; }

}