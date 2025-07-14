using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
internal class UpdateEvolucionSpecification : BaseSpecification<Evolucion>
{
    public UpdateEvolucionSpecification(int id)
         : base(e => e.Id == id && e.Borrado == false)
    {

    }
}
