using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
public class DetalleSucesoRelacionadoByIdSucesoPrincipalSpecification: BaseSpecification<DetalleSucesoRelacionado>
{
    public DetalleSucesoRelacionadoByIdSucesoPrincipalSpecification(int IdSucesoPrincipal)
        : base(
            d => d.SucesoRelacionado.IdSucesoPrincipal == IdSucesoPrincipal && 
            d.SucesoRelacionado.Borrado == false &&
            d.Borrado == false)
    {
    }
}
