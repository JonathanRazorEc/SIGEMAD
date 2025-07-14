using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Impactos;
public class ImpactoActiveByIdSpecification : BaseSpecification<ImpactoEvolucion>
{
    public ImpactoActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {
    }
}
