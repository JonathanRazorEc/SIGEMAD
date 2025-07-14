using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Impactos;
public class ImpactoEvolucionActiveSpecification : BaseSpecification<ImpactoEvolucion>
{
    public ImpactoEvolucionActiveSpecification()
        : base(i => i.Borrado == false)
    {
    }
}
