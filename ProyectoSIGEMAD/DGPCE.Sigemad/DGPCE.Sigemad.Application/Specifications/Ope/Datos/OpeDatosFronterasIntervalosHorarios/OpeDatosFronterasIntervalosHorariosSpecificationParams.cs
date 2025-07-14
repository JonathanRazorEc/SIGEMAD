namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronterasIntervalosHorarios;

public class OpeDatosFronterasIntervalosHorariosSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    public TimeSpan? Inicio { get; set; }
    public TimeSpan? Fin { get; set; }
    public bool? Borrado { get; set; }

}