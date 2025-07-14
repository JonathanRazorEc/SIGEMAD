using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
public class SucesosRelacionadosActiveByIdPrincipalSpecification: BaseSpecification<SucesoRelacionado>
{
    public SucesosRelacionadosActiveByIdPrincipalSpecification(int id, List<int> detallesSucesosRelacionados)
        : base(b => b.Id == id && b.Borrado == false)
    {
        AddInclude(b => b.DetalleSucesoRelacionados.Where(dir => detallesSucesosRelacionados.Contains(dir.IdSucesoAsociado) && !dir.Borrado));
        AddInclude("DetalleSucesoRelacionados.SucesoAsociado");
        AddInclude("DetalleSucesoRelacionados.SucesoAsociado.TipoSuceso");
        AddInclude("DetalleSucesoRelacionados.SucesoAsociado.Incendios.EstadoSuceso");
    }
}
