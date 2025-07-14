using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
internal class SucesosRelacionadosActiveByIdSucesoPrincipalSpecification : BaseSpecification<SucesoRelacionado>
{
    public SucesosRelacionadosActiveByIdSucesoPrincipalSpecification(int idSucesoPrincipal)
        : base(b => b.IdSucesoPrincipal == idSucesoPrincipal && b.Borrado == false)
    {
        AddInclude(b => b.DetalleSucesoRelacionados);
        AddInclude("DetalleSucesoRelacionados.SucesoAsociado");
        AddInclude("DetalleSucesoRelacionados.SucesoAsociado.TipoSuceso");
        AddInclude("DetalleSucesoRelacionados.SucesoAsociado.Incendios.EstadoSuceso");
    }
}
