namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLogs;

public class OpeLogsSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    public DateOnly? FechaRegistro { get; set; }
    public string? TipoMovimiento { get; set; }
   
}