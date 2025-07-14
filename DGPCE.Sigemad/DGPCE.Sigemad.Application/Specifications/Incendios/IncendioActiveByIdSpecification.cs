using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Incendios;
public class IncendioActiveByIdSpecification : BaseSpecification<Incendio>
{
    public IncendioActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {
        AddInclude(p => p.Suceso!);
    }
    
}
