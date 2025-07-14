namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;

public class OpePuertosSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    public string? Nombre { get; set; } = null!;
    public int? IdOpeFase { get; set; }
    public int? IdPais { get; set; }
    public int? IdCcaa { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }
    public int? CoordenadaUTM_X { get; set; }
    public int? CoordenadaUTM_Y { get; set; }
    public int? IdComparativoFecha { get; set; }
    public DateOnly? FechaValidezDesde { get; set; }
    public DateOnly? FechaValidezHasta { get; set; }
    public int? Capacidad { get; set; }

}