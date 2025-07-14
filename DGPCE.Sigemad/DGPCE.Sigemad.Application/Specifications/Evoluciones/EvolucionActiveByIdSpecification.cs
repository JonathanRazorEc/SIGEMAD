using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class EvolucionActiveByIdSpecification: BaseSpecification<Evolucion>
{
    public EvolucionActiveByIdSpecification(int id)
        : base(e => e.Id == id && e.Borrado == false)
    {   
    }
}
