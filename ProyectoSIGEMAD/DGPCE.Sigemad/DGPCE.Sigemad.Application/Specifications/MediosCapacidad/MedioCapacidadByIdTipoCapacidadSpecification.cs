

namespace DGPCE.Sigemad.Application.Specifications.MediosCapacidad;

public class MedioCapacidadByIdTipoCapacidadSpecification : BaseSpecification<Domain.Modelos.MediosCapacidad>
{
    public MedioCapacidadByIdTipoCapacidadSpecification(int idTipoCapacidad)
        : base(i => i.IdTipoCapacidad == idTipoCapacidad)
    {
        AddInclude(e => e.TipoCapacidad);
        AddInclude(e => e.TipoMedio);
    }

 
}
