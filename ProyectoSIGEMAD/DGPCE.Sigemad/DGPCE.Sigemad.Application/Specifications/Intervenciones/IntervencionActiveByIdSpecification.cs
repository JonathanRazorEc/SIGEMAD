using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Intervenciones;
internal class IntervencionActiveByIdSpecification : BaseSpecification<IntervencionMedio>
{
    public IntervencionActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
