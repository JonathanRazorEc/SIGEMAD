using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
public class SucesoRelacionadoActiveByIdPrincipalAndIdAsociadoSpecification : BaseSpecification<SucesoRelacionado>
{
    public SucesoRelacionadoActiveByIdPrincipalAndIdAsociadoSpecification(int idSucesoPrincipal, int idSucesoAsociado)
        : base(b => b.IdSucesoPrincipal == idSucesoPrincipal && b.Borrado == false)
    {
    }
}
