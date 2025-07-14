using System.Text.Json.Serialization;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodos;

public class OpePeriodosSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    public string? Nombre { get; set; } = null!;
    public int? IdOpePeriodoTipo { get; set; }
    public int? IdComparativoFecha { get; set; }
    public DateOnly? FechaInicioFaseSalida { get; set; }
    public DateOnly? FechaFinFaseSalida { get; set; }
    public DateOnly? FechaInicioFaseRetorno { get; set; }
    public DateOnly? FechaFinFaseRetorno { get; set; }

}