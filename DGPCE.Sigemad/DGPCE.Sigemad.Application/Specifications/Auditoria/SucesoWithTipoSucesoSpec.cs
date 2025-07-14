using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;

public class SucesoWithTipoSucesoSpec : BaseSpecification<Suceso>
{
    public SucesoWithTipoSucesoSpec(int idSuceso)
        : base(s => s.Id == idSuceso)
    {
        AddInclude(s => s.TipoSuceso);
    }
}
