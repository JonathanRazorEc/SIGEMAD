using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class SucesoRelacionado :BaseDomainModel<int>
{
    public SucesoRelacionado()
    {
        DetalleSucesoRelacionados = new List<DetalleSucesoRelacionado>();
    }

    public int IdSucesoPrincipal { get; set; }
    public virtual Suceso SucesoPrincipal { get; set; }

    public virtual List<DetalleSucesoRelacionado> DetalleSucesoRelacionados { get; set; }
}
