using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
internal class SucesosRelacionadosWithDetails : BaseSpecification<SucesoRelacionado>
{
    public SucesosRelacionadosWithDetails(SucesoRelacionadoParams @params)
    : base(d =>
    (!@params.Id.HasValue || d.Id == @params.Id) &&
    (!@params.IdSucesoPrincipal.HasValue || d.IdSucesoPrincipal == @params.IdSucesoPrincipal) &&
     d.Borrado == false)
    {

        AddInclude(b => b.DetalleSucesoRelacionados);
        AddInclude("DetalleSucesoRelacionados.SucesoAsociado");
        AddInclude("DetalleSucesoRelacionados.SucesoAsociado.TipoSuceso");
        AddInclude("DetalleSucesoRelacionados.SucesoAsociado.Incendios.EstadoSuceso");

    }
}
