namespace DGPCE.Sigemad.Application.Specifications.Registros;
public class RegistroSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    public int? IdSuceso { get; set; }
    public int? IdEntradaSalida { get; set; }
    public int? IdMedio { get; set; }
    public int? IdTipoRegistro { get; set; }
    public int? IdEstadoIncendio { get; set; }
    public int? IdSituacionOperativa { get; set; }

    public DateTime? FechaRegistro { get; set; }
}
