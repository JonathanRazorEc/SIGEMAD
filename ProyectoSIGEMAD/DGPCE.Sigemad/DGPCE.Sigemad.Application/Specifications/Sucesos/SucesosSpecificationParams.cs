namespace DGPCE.Sigemad.Application.Specifications.Sucesos;
public class SucesosSpecificationParams : SpecificationParams
{
    public string? Denominacion { get; set; }
    public int? IdClaseSuceso { get; set; }
    public int? IdTerritorio { get; set; }
    public int? IdPais { get; set; }
    public int? IdCcaa { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }
    public int? IdMovimiento { get; set; }
    public int? IdComparativoFecha { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public int? IdSuceso { get; set; }

}
