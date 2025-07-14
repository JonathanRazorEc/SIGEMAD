using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Capacidades;
public class GetAllCapacidadesSpecification : BaseSpecification<Capacidad>
{
    public GetAllCapacidadesSpecification()
    {
        AddInclude(c => c.TipoCapacidad);
        AddInclude(c => c.Entidad);
        AddInclude(c => c.Entidad.Organismo);
        AddInclude(c => c.Entidad.Organismo.Administracion);
        AddInclude(c => c.Entidad.Organismo.Administracion.TipoAdministracion);
    }
}
